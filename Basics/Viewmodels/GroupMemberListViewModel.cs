// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Supporters:
// ඞ Hackl Tobias
// ඞ Ratzenböck Peter

using Basics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basics.Viewmodels
{
    public class GroupMemberListViewModel : BaseViewModel
    {
        public List<User> Participants { get; }
        public string ChatroomName { get; }

        public GroupMemberListViewModel(List<User> patricipants, string chatroomName)
        {
            Participants = patricipants;
            ChatroomName = chatroomName + " participants";
        }
    }
}
