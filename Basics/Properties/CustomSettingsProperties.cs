// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Basics.Viewmodels;
using System;
using System.Collections.ObjectModel;

namespace Basics.Properties
{
    [Serializable]
    class CustomSettingsProperties
    {
        public ObservableCollection<ChatRoomViewModel> ChatRoomViewModels { get; set; }

        public CustomSettingsProperties()
        {
            ChatRoomViewModels = new ObservableCollection<ChatRoomViewModel>();
        }
    }
}
