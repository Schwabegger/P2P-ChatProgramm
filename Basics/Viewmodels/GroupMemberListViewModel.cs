﻿// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Basics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Basics.Viewmodels
{
    public class GroupMemberListViewModel : BaseViewModel
    {
        public List<User> Participants { get; }
        public string ChatroomName { get; }

        public GroupMemberListViewModel(List<User> patricipants, string chatroomName)
        {
            Participants = patricipants;
            ChatroomName = chatroomName + " " + Application.Current.FindResource("GroupMemberListPartTitle");
        }
    }
}
