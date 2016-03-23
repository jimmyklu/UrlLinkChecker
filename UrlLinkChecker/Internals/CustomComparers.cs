namespace UrlLinkChecker.Internals
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms;

    internal class ListViewItemComparer : IComparer
    {
        private int col;
        private SortOrder order;
        public ListViewItemComparer()
        {
            col = 0;
            order = SortOrder.Ascending;
        }
        public ListViewItemComparer(int column, SortOrder order)
        {
            col = column;
            this.order = order;
        }
        public int Compare(object x, object y)
        {
            int returnVal = -1;
            returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                                    ((ListViewItem)y).SubItems[col].Text);

            if (order == SortOrder.Descending)
            {
                returnVal *= -1;
            }

            return returnVal;
        }
    }


    internal class UrlComparer : IEqualityComparer<ListViewItem>, IComparer<ListViewItem>, IComparer
    {
        public bool Equals(ListViewItem x, ListViewItem y)
        {
            return x.SubItems[0].Text.Equals(y.SubItems[0].Text);
        }

        public int GetHashCode(ListViewItem obj)
        {
            return obj.SubItems[0].Text.GetHashCode();
        }

        public int Compare(object x, object y)
        {
            return this.Compare((ListViewItem)x, (ListViewItem)y);
        }

        public int Compare(ListViewItem x, ListViewItem y)
        {
            return x.SubItems[0].Text.CompareTo(y.SubItems[0].Text);
        }
    }

    internal class ResultComparer : IEqualityComparer<ListViewItem>, IComparer<ListViewItem>, IComparer
    {
        public bool Equals(ListViewItem x, ListViewItem y)
        {
            bool areEqual = x.SubItems[1].Text.Equals(y.SubItems[1].Text);

            if (areEqual)
            {
                areEqual = x.SubItems[0].Text.Equals(y.SubItems[0].Text);
            }

            return areEqual;
        }

        public int GetHashCode(ListViewItem obj)
        {
            return obj.SubItems[1].Text.GetHashCode();
        }

        public int Compare(object x, object y)
        {
            return this.Compare((ListViewItem)x, (ListViewItem)y);
        }

        public int Compare(ListViewItem x, ListViewItem y)
        {
            int retVal = x.SubItems[1].Text.CompareTo(y.SubItems[1].Text);

            if (retVal == 0)
            {
                retVal = x.SubItems[0].Text.CompareTo(y.SubItems[0].Text);
            }

            return retVal;
        }
    }

    internal class ErrorComparer : IEqualityComparer<ListViewItem>, IComparer<ListViewItem>, IComparer
    {
        public bool Equals(ListViewItem x, ListViewItem y)
        {
            bool areEqual = x.SubItems[2].Text.Equals(y.SubItems[2].Text);

            if (areEqual)
            {
                areEqual = x.SubItems[0].Text.Equals(y.SubItems[0].Text);
            }

            return areEqual;
        }

        public int GetHashCode(ListViewItem obj)
        {
            return obj.SubItems[2].Text.GetHashCode() + obj.SubItems[0].Text.GetHashCode();
        }

        public int Compare(object x, object y)
        {
            return this.Compare((ListViewItem)x, (ListViewItem)y);
        }

        public int Compare(ListViewItem x, ListViewItem y)
        {
            int retVal = x.SubItems[2].Text.CompareTo(y.SubItems[2].Text);

            if (retVal == 0)
            {
                retVal = x.SubItems[0].Text.CompareTo(y.SubItems[0].Text);
            }

            return retVal;
        }
    }

}
