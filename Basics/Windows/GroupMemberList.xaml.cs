// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Supporters:
// ඞ Hackl Tobias
// ඞ Ratzenböck Peter

using Basics.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Basics.Windows
{
    /// <summary>
    /// Interaction logic for GroupMemberList.xaml
    /// </summary>
    public partial class GroupMemberList : Window
    {
        public GroupMemberListViewModel GroupMemberListViewModel
        {
            get { return (GroupMemberListViewModel)GetValue(GroupMemberListViewModelProperty); }
            set { SetValue(GroupMemberListViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupMemberListViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupMemberListViewModelProperty =
            DependencyProperty.Register("GroupMemberListViewModel", typeof(GroupMemberListViewModel), typeof(GroupMemberList), new PropertyMetadata(null));

        public GroupMemberList()
        {
            InitializeComponent();
        }
    }
}
