using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Text;

namespace LandmarkDevs.Core.Shared
{
    public static class Extensions
    {
        #region String Extensions
        /// <summary>
        /// Returns the characters of the string from the left by the specified length.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        public static string Left(this string str, int length)
        {
            return str.Substring(0, length);
        }

        /// <summary>
        /// Returns the characters of the string from the right by the specified length.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        public static string Right(this string str, int length)
        {
            var revString = str.Reverse();
            var rightString = revString.Left(length);
            return rightString.Reverse();
        }

        /// <summary>
        /// Reverses the specified string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>System.String.</returns>
        public static string Reverse(this string str)
        {
            var sb = new StringBuilder();
            var sArray = str.ToCharArray();
            for (var i = sArray.Length - 1; i >= 0; i--)
            {
                sb.Append(sArray[i]);
            }
            return sb.ToString();
        }

        #endregion

        #region Date Extensions
        /// <summary>
        /// Converts the DateTime to a Date ID.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>System.Int64.</returns>
        public static long ToDateId(this DateTime date)
        {
            var shortDate = date.ToShortDateString();
            var vals = shortDate.Split('/');
            var idStr = vals[2];
            if (vals[0].Length == 1)
                idStr = idStr + "0" + vals[0];
            else idStr += vals[0];
            if (vals[1].Length == 1)
                idStr = idStr + "0" + vals[1];
            else idStr += vals[1];
            return Convert.ToInt64(idStr);
        }

        /// <summary>
        /// Converts a Date ID to a string.
        /// </summary>
        /// <param name="dateId">The date identifier.</param>
        /// <returns>System.String.</returns>
        public static string ToDateString(this long dateId)
        {
            var idStr = dateId.ToString();
            var dateStr = idStr.Substring(4, 2) + "/";
            dateStr = dateStr + idStr.Substring(6, 2) + "/";
            dateStr += idStr.Substring(0, 4);
            return dateStr;
        }

        /// <summary>
        /// Converts a Date ID to DateTime
        /// </summary>
        /// <param name="dateId">The date identifier.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ToDateTime(this long dateId)
        {
            return Convert.ToDateTime(dateId.ToDateString());
        }

        /// <summary>
        /// Adds/Subtracts the work days.
        /// Add the following code to include holidays in the calculation.
        /// <example>
        /// if (newDate.DayOfWeek != DayOfWeek.Saturday &&
        /// newDate.DayOfWeek != DayOfWeek.Sunday && !newDate.IsHoliday())
        /// {
        /// workingDays -= direction;
        /// }
        /// </example>
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="workingDays">The working days.</param>
        /// <returns>DateTime.</returns>
        public static DateTime AddWorkDays(this DateTime date, int workingDays)
        {
            var direction = workingDays < 0 ? -1 : 1;
            var newDate = date;
            while (workingDays != 0)
            {
                newDate = newDate.AddDays(direction);
                if (newDate.DayOfWeek != DayOfWeek.Saturday
                    && newDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays -= direction;
                }
            }
            return newDate;
        }

        #endregion

        #region Excel and DataTable Extensions
        /// <summary>
        /// Gets the Excel file connection string.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.String.</returns>
        public static string GetExcelFileConnectionString(string file)
        {
            var props = file.EndsWith("X") || file.EndsWith("x") ? new Dictionary<string, string>
            {
                ["Provider"] = "Microsoft.ACE.OLEDB.12.0",
                ["Extended Properties"] = "Excel 12.0 XML",
                ["Data Source"] = file
            } : new Dictionary<string, string>
            {
                ["Provider"] = "Provider=Microsoft.Jet.OLEDB.4.0",
                ["Extended Properties"] = "Excel 8.0 XML;HDR=YES",
                ["Data Source"] = file
            };
            var sb = new StringBuilder();
            foreach (var prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts an Excel spreadsheet to a datatable. The spreadsheet must
        /// have a .XLSX extension.
        /// </summary>
        /// <param name="spreadsheetPath">The spreadsheet path.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <returns>DataTable.</returns>
        public static DataTable ConvertSpreadsheetToDataTable(string spreadsheetPath, string sheetName)
        {
            var datatable = new DataTable();
            IXLWorksheet xlWorksheet;
            using (var workbook = new XLWorkbook(spreadsheetPath))
            {
                xlWorksheet = workbook.Worksheet(sheetName);
            }
            var range = xlWorksheet.Range(xlWorksheet.FirstCellUsed(), xlWorksheet.LastCellUsed());
            var col = range.ColumnCount();
            datatable.Clear();
            for (var i = 1; i <= col; i++)
            {
                var column = xlWorksheet.Cell(1, i);
                datatable.Columns.Add(column.Value.ToString());
            }
            var firstHeadRow = 0;
            foreach (var item in range.Rows())
            {
                if (firstHeadRow != 0)
                {
                    var array = new object[col];
                    for (var y = 1; y <= col; y++)
                    {
                        try
                        {
                            array[y - 1] = item.Cell(y).Value;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message.Trim());
                            break;
                        }
                    }
                    datatable.Rows.Add(array);
                }
                firstHeadRow++;
            }
            return datatable;
        }

        /// <summary>
        /// Writes the datatable to a CSV file.
        /// </summary>
        /// <param name="dt">The datatable.</param>
        /// <param name="filePath">The file path.</param>
        public static void WriteToCsv(DataTable dt, string filePath)
        {
            var sb = new StringBuilder();
            var columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));
            foreach (DataRow row in dt.Rows)
            {
                var fields = row.ItemArray.Select(field => string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));
            }
            File.WriteAllText(filePath, sb.ToString());
        }

        #endregion

        #region Object Extensions
        /// <summary>
        /// Clones the list of T.
        /// </summary>
        /// <typeparam name="T">The Type.</typeparam>
        /// <param name="list">The list that will be cloned</param>
        /// <returns></returns>
        public static List<T> Clone<T>(this List<T> list) where T : ICloneable
        {
            return list.Select(item => (T)item.Clone()).ToList();
        }

        #endregion

        #region Active Directory Extensions
        /// <summary>
        /// Searches the active directory for the user.
        /// </summary>
        /// <param name="directoryName">The name of the active directory.</param>
        /// <returns>DirectoryEntry.</returns>
        public static DirectoryEntry SearchDirectoryForUserEntry(string directoryName)
        {
            if (Environment.UserDomainName != directoryName)
                return null;
            var userName = UserPrincipal.Current.Name;
            using (var ds = new DirectorySearcher())
            {
                ds.Filter = "(&(objectClass=user) (cn=" + userName + "))";
                var usr = ds.FindOne();
                return usr == null ? null : new DirectoryEntry(usr.Path);
            }
        }

        #endregion

        #region JSON and File Extensions
        /// <summary>
        /// Saves to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The file path.</param>
        /// <param name="obj">The object.</param>
        public static void SaveToJson<T>(string filePath, T obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            });
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    stream.Position = 0;
                    stream.CopyTo(fs);
                }
            }
        }

        /// <summary>
        /// Converts the Json string to an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns>T.</returns>
        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Converts the object to Json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerableObject">The enumerable object.</param>
        /// <returns>System.String.</returns>
        public static string ToJson<T>(this T enumerableObject)
        {
            return JsonConvert.SerializeObject(enumerableObject);
        }

        /// <summary>
        /// Writes the text to file.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="path">The path.</param>
        public static void WriteTextToFile(string text, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(text);
                writer.Flush();
                writer.Close();
            }
        }

        #endregion
    }
}