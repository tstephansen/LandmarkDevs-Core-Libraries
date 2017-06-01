using Prism.Regions;
using System;
using System.Collections.Generic;

namespace LandmarkDevs.Core.Prism
{
    /// <summary>
    /// Class RegionNavigationJournalWrapper.
    /// </summary>
    /// <seealso cref="IRegionNavigationJournal" />
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RegionNavigationJournalWrapper : IRegionNavigationJournal
    {
        private readonly IRegionNavigationJournal _regionNavigationJournal;
        private readonly Stack<Uri> _backStack = new Stack<Uri>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionNavigationJournalWrapper" /> class.
        /// Constructor inject prism default RegionNavigationJournal to wrap
        /// </summary>
        /// <param name="regionNavigationJournal">The region navigation journal.</param>
        public RegionNavigationJournalWrapper(RegionNavigationJournal regionNavigationJournal)
        {
            _regionNavigationJournal = regionNavigationJournal;
        }

        /// <summary>
        /// Gets the name of the previous view.
        /// </summary>
        /// <value>The name of the previous view.</value>
        public string PreviousViewName => _backStack.Count > 0 ? _backStack.Peek().OriginalString : string.Empty;

        /// <summary>
        /// Gets a value indicating whether this instance can go back.
        /// </summary>
        /// <value><c>true</c> if this instance can go back; otherwise, <c>false</c>.</value>
        public bool CanGoBack => _regionNavigationJournal.CanGoBack;

        /// <summary>
        /// Gets a value indicating whether this instance can go forward.
        /// </summary>
        /// <value><c>true</c> if this instance can go forward; otherwise, <c>false</c>.</value>
        public bool CanGoForward => _regionNavigationJournal.CanGoForward;

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _backStack.Clear();
            _regionNavigationJournal.Clear();
        }

        /// <summary>
        /// Gets the current entry.
        /// </summary>
        /// <value>The current entry.</value>
        public IRegionNavigationJournalEntry CurrentEntry => _regionNavigationJournal.CurrentEntry;

        /// <summary>
        /// Goes back.
        /// </summary>
        public void GoBack()
        {
            // Save current entry
            var currentEntry = CurrentEntry;
            // try and go back
            _regionNavigationJournal.GoBack();
            // if currententry isn't equal to previous entry then we moved back
            if (CurrentEntry != currentEntry)
            {
                _backStack.Pop();
            }
        }

        /// <summary>
        /// Goes forward.
        /// </summary>
        public void GoForward()
        {
            // Save current entry
            var currentEntry = CurrentEntry;
            // try and go forward
            _regionNavigationJournal.GoForward();
            // if currententry isn't equal to previous entry then we moved forward
            if (currentEntry != null && CurrentEntry != currentEntry)
            {
                _backStack.Push(currentEntry.Uri);
            }
        }

        /// <summary>
        /// Gets or sets the navigation target.
        /// </summary>
        /// <value>The navigation target.</value>
        public INavigateAsync NavigationTarget
        {
            get { return _regionNavigationJournal.NavigationTarget; }
            set { _regionNavigationJournal.NavigationTarget = value; }
        }

        /// <summary>
        /// Records the navigation.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public void RecordNavigation(IRegionNavigationJournalEntry entry)
        {
            var currentEntry = CurrentEntry;
            _regionNavigationJournal.RecordNavigation(entry);
            // if currententry isn't equal to previous entry then we moved forward
            if (currentEntry != null && CurrentEntry == entry)
            {
                _backStack.Push(currentEntry.Uri);
            }
        }
    }
}