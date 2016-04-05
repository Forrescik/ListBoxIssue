using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ListBoxIssue.ViewModel
{
    public class MessageViewModel : ViewModelBase
    {

        private RelayCommand _addNewItems;

        public RelayCommand AddNewItems
        {
            get { return _addNewItems ?? new RelayCommand(AddItems); }
        }

        private RelayCommand _addNewMessage;
        public RelayCommand AddNewMessage { get { return _addNewMessage ?? new RelayCommand(NewMessage); } }

        private void NewMessage()
        {
            MessageViewModels.Add(new MessageModel
            {
                Description = "new item"
            });
        }

        private int _itemsCounter;

        public int ItemsCounter
        {
            get { return _itemsCounter; }
            set
            {
                _itemsCounter = value;
                RaisePropertyChanged(() => ItemsCounter);
            }
        }

        private void AddItems()
        {
            for (int i = 0; i < 15; i++)
            {
                MessageViewModels.Insert(0, new MessageModel
                {
                    Description = "seba" + i
                });
            }
            ItemsCounter = 15;
            RaisePropertyChanged(() => ItemsCounter);
        }

        private ObservableCollection<MessageModel> _messageViewModels;

        public ObservableCollection<MessageModel> MessageViewModels
        {
            get { return _messageViewModels; }
            set
            {
                if (_messageViewModels == value)
                    return;
                _messageViewModels = value;
                RaisePropertyChanged(() => MessageViewModels);
            }
        }

        private void InitializeList2()
        {
            MessageViewModels = new ObservableCollection<MessageModel>
            {
                 new MessageModel
                {
                    Description = "test2",
                },
                  new MessageModel
                {
                    Description = "test1",
                },
            };
        }

        private void InitializeList()
        {
            MessageViewModels = new ObservableCollection<MessageModel>
            {
                new MessageModel
                {
                    Description = "test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1",
                },
                new MessageModel
                {
                    Description = "test2",
                },
                new MessageModel
                {
                    Description = "test3",
                },
                new MessageModel
                {
                    Description = "test4",
                },
                new MessageModel
                {
                    Description = "test5",
                },
                new MessageModel
                {
                    Description = "test6",
                },
                new MessageModel
                {
                    Description = "test7",
                },
                new MessageModel
                {
                    Description = "test8",
                },
                new MessageModel
                {
                    Description = "test9",
                },
                new MessageModel
                {
                    Description = "test10",
                },
                new MessageModel
                {
                    Description = "test11",
                },
                new MessageModel
                {
                    Description = "test12",
                },
                new MessageModel
                {
                    Description = "test13",
                },
                new MessageModel
                {
                    Description = "test14",
                },
                new MessageModel
                {
                    Description = "test15",
                },
                new MessageModel
                {
                    Description = "test16",
                },
                new MessageModel
                {
                    Description = "test17",
                },
                new MessageModel
                {
                    Description = "test18",
                },
                new MessageModel
                {
                    Description = "test19",
                },
                new MessageModel
                {
                    Description = "test20",
                },
                new MessageModel
                {
                    Description = "test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1test1",
                },
                new MessageModel
                {
                    Description = "test2",
                },
                new MessageModel
                {
                    Description = "test3",
                },
                new MessageModel
                {
                    Description = "test4",
                },
                new MessageModel
                {
                    Description = "test5",
                },
                new MessageModel
                {
                    Description = "test6",
                },
                new MessageModel
                {
                    Description = "test7",
                },
                new MessageModel
                {
                    Description = "test8",
                },
                new MessageModel
                {
                    Description = "test9",
                },
                new MessageModel
                {
                    Description = "test10",
                },
                new MessageModel
                {
                    Description = "test11",
                },
                new MessageModel
                {
                    Description = "test12",
                },
                new MessageModel
                {
                    Description = "test13",
                },
                new MessageModel
                {
                    Description = "test14",
                },
                new MessageModel
                {
                    Description = "test15",
                },
                new MessageModel
                {
                    Description = "test16",
                },
                new MessageModel
                {
                    Description = "test17",
                },
                new MessageModel
                {
                    Description = "test18",
                },
                new MessageModel
                {
                    Description = "test19",
                },
                new MessageModel
                {
                    Description = "test20",
                }
            };
        }

        public MessageViewModel()
        {
            InitializeList();
        }
    }
}
