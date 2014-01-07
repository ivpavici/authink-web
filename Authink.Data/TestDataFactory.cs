﻿using System;
﻿using System.Linq;

﻿using db = Authink.Data;

namespace Authink.Data.Factory
{
    public static class TestDataFactory
    {
        public static void Fill()
        {
            try
            {
                using (var dataModel = new db::AuthinkDataModel())
                {
                    if (!dataModel.Users.Any())
                    {
                        InsertUserData(dataModel);
                        dataModel.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        private static void InsertUserData(db.AuthinkDataModel dataModel)
        {
            new[]
                {
                    new db::User
                    {
                        Id = 1,
                        Username  = "authink",
                        Password  = "9EFE9A5A1DA5F41A4EB7599F2715DC24ABF5BBC8",
                        Email     = "info@authink.org",
                        FirstName = "Team",
                        LastName  = "Authink",
                        IsHidden = false,
                        Children  = {   
                                        new db::Child
                                            {
                                                FirstName = "Marry-ann",   
                                                LastName = "Jennings",  
                                                IsHidden = false,
                                                ProfilePictureUrl ="http://data.authink.eu/Content/Images/default-child.png",
                                                Tests  = {
                                                    new db::Test
                                                              {
                                                                  UserId = 1,
                                                                  IsDeleted = false,
                                                                  Name = "Continue sequence",
                                                                  LongDescription = "The objective of this test is to recognize through three tasks which items continue a given sequence of items. This ecxercise helps improve childs logical reasoning. ",
                                                                  ShortDescription = "The objective of this test is to recognize through three tasks which items continue a given sequence of items.",
                                                                  Tasks =
                                                                      {
                                                                          new db::Task
                                                                              {
                                                                                  Id = 1,
                                                                                  UserId = 1,
                                                                                  Name = "Continue sequence, Difficulty 1",
                                                                                  Description = "In this task the objective is to recognize which item continues the sequence of two items. The task can be solved by clicking on the correct item in the vertical list.",
                                                                                  IsHidden = false,
                                                                                  Type = "#004",
                                                                                  Difficulty = 5,
                                                                                  ProfilePictureUrl = "http://data.authink.eu/Content/Images/TaskIcons/task-continue-sequence-256x256.png",
                                                                                  Sound = new db::Sound{IsHidden = false, Type = "ttl", Url = "http://data.authink.eu/Content/Sounds/default-voice-command.mp3"},
                                                                                  Pictures =
                                                                                      {
                                                                                          new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/1/bowl.png",
                                                                                                  IsAnswer = false
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/1/casserole.png",
                                                                                                  IsAnswer = false
                                                                                              }
                                                                                     
                                                                                      }
                                                                              },
                                                                              new db::Task
                                                                              {
                                                                                  Id = 2,
                                                                                  UserId = 1,
                                                                                  Name = "Continue sequence, Difficulty 2",
                                                                                  Description = "In this task the objective is to recognize which item continues the sequence of three items. The task can be solved by clicking on the correct item in the vertical list.",
                                                                                  IsHidden = false,
                                                                                  Type = "#004",
                                                                                  Difficulty = 5,
                                                                                  ProfilePictureUrl = "http://data.authink.eu/Content/Images/TaskIcons/task-continue-sequence-256x256.png",
                                                                                  Sound = new db::Sound{IsHidden = false, Type = "ttl", Url = "http://data.authink.eu/Content/Sounds/default-voice-command.mp3"},
                                                                                  Pictures =
                                                                                      {
                                                                                          new db::Picture
                                                                                               {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/2/bowl.png",
                                                                                                  IsAnswer = false
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/2/casserole.png",
                                                                                                  IsAnswer = false
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/2/cup.png",
                                                                                                  IsAnswer = false,
                                                                                              }
                                                                                      }
                                                                              },
                                                                              new db::Task
                                                                              {
                                                                                  Id = 3,
                                                                                  UserId = 1,
                                                                                  Name = "Continue sequence, Difficulty 3",
                                                                                  Description = "In this task the objective is to recognize which item continues the sequence of four items. The task can be solved by clicking on the correct item in the vertical list.",
                                                                                  IsHidden = false,
                                                                                  Type = "#004",
                                                                                  Difficulty = 5,
                                                                                  ProfilePictureUrl = "http://data.authink.eu/Content/Images/TaskIcons/task-continue-sequence-256x256.png",
                                                                                  Sound = new db::Sound{IsHidden = false, Type = "ttl", Url = "http://data.authink.eu/Content/Sounds/default-voice-command.mp3"},
                                                                                  Pictures =
                                                                                      {
                                                                                          new db::Picture
                                                                                               {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/3/bowl.png",
                                                                                                  IsAnswer = false
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/3/casserole.png",
                                                                                                  IsAnswer = false
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/3/cup.png",
                                                                                                  IsAnswer = false
                                                                                              },
                                                                                                   new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/3/frying pan.png",
                                                                                                  IsAnswer = false
                                                                                              }
                                                                                     
                                                                                      }
                                                                              }
                                                                      }

                                                              },
                                                new db::Test
                                                              {
                                                                  UserId = 1,
                                                                  IsDeleted = false,
                                                                  Name = "Detect colors",
                                                                  LongDescription = "The objective of this test is to recognize through three tasks the colors of given items.This ecxercise helps improve childs color recognision.",
                                                                  ShortDescription = "The objective of this test is to recognize through three tasks the colors of given items.",
                                                                  Tasks =
                                                                      {
                                                                          new db::Task
                                                                              {
                                                                                  Id = 4,
                                                                                  UserId = 1,
                                                                                  Name = "Detect colors, Difficulty 1",
                                                                                  Description = "In this task the objective is to recognize the color of each of the six given items. The choice is between two given colors.",
                                                                                  IsHidden = false,
                                                                                  Type = "#003",
                                                                                  Difficulty = 4,
                                                                                  ProfilePictureUrl = "http://data.authink.eu/Content/Images/TaskIcons/task-detect-colors-256x256.png",
                                                                                  Sound = new db::Sound{IsHidden = false, Type = "ttl", Url = "http://data.authink.eu/Content/Sounds/default-voice-command.mp3"},
                                                                                  Pictures =
                                                                                      {
                                                                                          new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/4/bowl.png",
                                                                                                  IsAnswer = true,
                                                                                                  Colors =
                                                                                                      {
                                                                                                          new db::Color {IsCorrect = true,PictureId = 16,Value = "#00B2EE"},
                                                                                                          new db::Color {IsCorrect = false,PictureId = 16,Value = "#838B8B"}
                                                                                                      },
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/4/red-casserole.png",
                                                                                                  IsAnswer = true,
                                                                                                  Colors =
                                                                                                      {
                                                                                                          new db::Color {IsCorrect = true,PictureId = 17, Value = "#DB2929"},
                                                                                                          new db::Color {IsCorrect = false,PictureId = 17,Value = "#EEB4B4"}
                                                                                                      },
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/4/greenCup.png",
                                                                                                  IsAnswer = true,
                                                                                                  Colors =
                                                                                                      {
                                                                                                          new db::Color {IsCorrect = true,PictureId = 17, Value = "#008000"},
                                                                                                          new db::Color {IsCorrect = false,PictureId = 17,Value = "#00B2EE"}
                                                                                                      },
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/4/yellow kitchen tool_3.png",
                                                                                                  IsAnswer = true,
                                                                                                  Colors =
                                                                                                      {
                                                                                                          new db::Color {IsCorrect = true, PictureId = 17, Value = "#FFFF00"},
                                                                                                          new db::Color {IsCorrect = false,PictureId = 17,Value = "#3B5323"}
                                                                                                      },
                                                                                              }
                                                                                      }
                                                                              },
                                                                              new db::Task
                                                                              {
                                                                                  Id = 5,
                                                                                  UserId = 1,
                                                                                  Name = "Detect colors, Difficulty 2",
                                                                                  Description = "In this task the objective is to recognize the color of each of the six given items. The choice is between two given colors.",
                                                                                  IsHidden = false,
                                                                                  Type = "#003",
                                                                                  Difficulty = 5,
                                                                                  ProfilePictureUrl = "http://data.authink.eu/Content/Images/TaskIcons/task-detect-colors-256x256.png",
                                                                                  Pictures =
                                                                                      {
                                                                                          new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/5/bowl.png",
                                                                                                  IsAnswer = true,
                                                                                                  Colors =
                                                                                                      {
                                                                                                          new db::Color {IsCorrect = true,PictureId = 16,Value = "#00B2EE"},
                                                                                                          new db::Color {IsCorrect = false,PictureId = 16,Value = "#838B8B"},
                                                                                                          new db::Color {IsCorrect = false,PictureId = 16,Value = "#A52A2A"},
                                                                                                          new db::Color {IsCorrect = false,PictureId = 16,Value = "#FFA500"}
                                                                                                      },
                                                                                                  
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/5/red-casserole.png",
                                                                                                  IsAnswer = true,
                                                                                                  Colors =
                                                                                                      {
                                                                                                          new db::Color {IsCorrect = true,PictureId = 17, Value = "#DB2929"},
                                                                                                          new db::Color {IsCorrect = false,PictureId = 17,Value = "#EEB4B4"},
                                                                                                          new db::Color {IsCorrect = false,PictureId = 16,Value = "##A52A2A"},
                                                                                                          new db::Color {IsCorrect = false,PictureId = 16,Value = "#FFA500"}
                                                                                                      },
                                                                                              }
                                                                                          
                                                                                      }
                                                                              }
                                                                      }
                                                              },
                                                               new db::Test
                                                              {
                                                                  UserId = 1,
                                                                  IsDeleted = false,
                                                                  Name = "Kitchen",
                                                                  LongDescription = "The objective of this test is to recognize items from the kitchen and learn their attributes. This test is a mix of color and item recognition ",
                                                                  ShortDescription = "The objective of this test is to help your child find his way around kitchen",
                                                                  Tasks =
                                                                      {
                                                                          new db::Task
                                                                              {
                                                                                  Id = 6,
                                                                                  UserId = 1,
                                                                                  Name = "Detect same kitchen items",
                                                                                  Description = "In this task the objective is to pair three items by dragging them from the bottom list next to the correct item.",
                                                                                  IsHidden = false,
                                                                                  Type = "#001",
                                                                                  Difficulty = 4,
                                                                                  ProfilePictureUrl = "http://data.authink.eu/Content/Images/TaskIcons/task-pair-same-items-256x256.png",
                                                                                  Pictures =
                                                                                      {
                                                                                          new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/6/kitchen tool_2.png",
                                                                                                  IsAnswer = true
                                                                                               
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/6/kitchen tool_3.png",
                                                                                                  IsAnswer = false
                                                                                               
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/6/kitchen tool_6.png",
                                                                                                  IsAnswer = false
                                                                                               
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/6/frying pan.png",
                                                                                                  IsAnswer = false
                                                                                               
                                                                                              },
                                                                                      }
                                                                              },
                                                                              new db::Task
                                                                              {
                                                                                  Id = 7,
                                                                                  UserId = 1,
                                                                                  Name = "Sort kitchen items by size",
                                                                                  Description = "In this test, through three tasks, the objective is to sort items by their size.",
                                                                                  IsHidden = false,
                                                                                  Type = "#008",
                                                                                  Difficulty = 5,
                                                                                  ProfilePictureUrl = "http://data.authink.eu/Content/Images/TaskIcons/task-order-by-size-256x256.png",
                                                                                  Pictures =
                                                                                      {
                                                                                          new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  IsAnswer = true,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/7/jug-3.png"
                                                                                                  
                                                                                              }
                                                                                      }
                                                                              },
                                                                              new db::Task
                                                                              {
                                                                                  Id = 8,
                                                                                  UserId = 1,
                                                                                  Name = "Show an item",
                                                                                  Description = "In this test, the objective is to show a correct item from a set of items, given by a voice command.",
                                                                                  IsHidden = false,
                                                                                  Type = "#009",
                                                                                  Difficulty = 5,
                                                                                  ProfilePictureUrl = "http://data.authink.eu/Content/Images/TaskIcons/task-voice-commands-256x256.png",
                                                                                  Pictures =
                                                                                      {
                                                                                          new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/8/casa.jpg",
                                                                                                  IsAnswer = true
                                                                                                  
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/8/tanjur.jpg",
                                                                                                  IsAnswer = false
                                                                                                  
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/8/tava.jpg",
                                                                                                  IsAnswer = false
                                                                                                  
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/8/ubrus.jpg",
                                                                                                  IsAnswer = false
                                                                                                  
                                                                                              }
                                                                                      }
                                                                              },
                                                                               new db::Task
                                                                              {
                                                                                  Id = 9,
                                                                                  UserId = 1,
                                                                                  Name = "Pair halves of kitchen items",
                                                                                  Description = "The objective of this test is to recognize and pair the halves of each given item through three tasks.",
                                                                                  IsHidden = false,
                                                                                  Type = "#005",
                                                                                  Difficulty = 5,
                                                                                  ProfilePictureUrl = "http://data.authink.eu/Content/Images/TaskIcons/task-pair-halves-256x256.png",
                                                                                  Pictures =
                                                                                      {
                                                                                          new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/9/bowl-left.png",
                                                                                                  IsAnswer = true
                                                                                              },
                                                                                               new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/9/casserole-left.png",
                                                                                                  IsAnswer = true
                                                                                                  
                                                                                              },
                                                                                               new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/9/bowl-right.png",
                                                                                                  IsAnswer = false
                                                                                                  
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/9/casserole-right.png",
                                                                                                  IsAnswer = false
                                                                                                  
                                                                                              }
                                                                                      }

                                                                              }

                                                                      }
                                                              },
                                                              new db::Test
                                                              {
                                                                  UserId = 1,
                                                                  IsDeleted = false,
                                                                  Name =  "Detect different items",
                                                                  LongDescription = "The objective of this test is to recognize through three tasks which items do not belong in a sequence of same items.This test can enforce logical recognition ",
                                                                  ShortDescription = "The objective of this test is to recognize through three tasks which items do not belong in a sequence of same items.",
                                                                  Tasks =
                                                                      {
                                                                          new db::Task
                                                                              {
                                                                                  Id = 10,
                                                                                  UserId = 1,
                                                                                  Name =  "Detect different item, Difficulty 1",
                                                                                  Description = "In this task the objective is to recognize an item that is different from others in a list of same items.",
                                                                                  IsHidden = false,
                                                                                  Type = "#002",
                                                                                  Difficulty = 4,
                                                                                  ProfilePictureUrl = "http://data.authink.eu/Content/Images/TaskIcons/task-detect-different-items-256x256.png",
                                                                                  Pictures =
                                                                                      {
                                                                                          new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  IsAnswer = true,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/10/bowl.png"
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  IsAnswer = true,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/10/casserole.png"
                                                                                              },
                                                                                             new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  IsAnswer = true,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/10/frying pan.png"
                                                                                              },
                                                                                             new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  IsAnswer = true,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/10/kitchen tool_2.png"
                                                                                              }
                                                                                      }
                                                                              },
                                                                              new db::Task
                                                                              {
                                                                                  Id = 11,
                                                                                  UserId = 1,
                                                                                  Name = "Detect different item, Difficulty  2",
                                                                                  Description = "In this task the objective is to recognize an item that is different in a set of scattered items.",
                                                                                  IsHidden = false,
                                                                                  Type = "#002",
                                                                                  Difficulty = 5,
                                                                                  ProfilePictureUrl = "http://data.authink.eu/Content/Images/TaskIcons/task-detect-different-items-256x256.png",
                                                                                  Pictures =
                                                                                      {
                                                                                          new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  IsAnswer = true,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/11/lonac.png"
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  IsAnswer = true,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/11/Red_cambridge_mug.png"
                                                                                              }
                                                                                          
                                                                                      }
                                                                              },
                                                                              new db::Task
                                                                              {
                                                                                  Id = 12,
                                                                                  UserId = 1,
                                                                                  Name =  "Detect different item, Difficulty  3",
                                                                                  Description = "In this task the objective is to recognize an item that is different, but similar to items in a set of scattered items.",
                                                                                  IsHidden = false,
                                                                                  Type = "#002",
                                                                                  Difficulty = 5,
                                                                                  ProfilePictureUrl = "http://data.authink.eu/Content/Images/TaskIcons/task-detect-different-items-256x256.png",
                                                                                  Pictures =
                                                                                      {
                                                                                          new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  IsAnswer = true,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/12/individual-red-mug.png"
                                                                                              },
                                                                                              new db::Picture
                                                                                              {
                                                                                                  Theme = "Kitchen",
                                                                                                  IsHidden = false,
                                                                                                  IsAnswer = true,
                                                                                                  Url = "http://data.authink.eu/Content/Images/Lite/12/Red_cambridge_mug.png"
                                                                                              }
                                                                                          
                                                                                      }
                                                                              }
                                                                      }
                                                              },
                                                }
                                     }
                        }
                    },
                    new db::User
                    {
                        Id = 2,
                        Username  = "djecaci",
                        Password  = "",
                        Email     = "info@info.org",
                        FirstName = "Djecaci",
                        LastName  = "Ludi",
                        IsHidden = false,
                        Children  = {   
                                        new db::Child{ FirstName = "Vojko", LastName = "Vrućina", IsHidden = false, ProfilePictureUrl = "Content/Images/Children/default-profile-picture.png"},
                                        new db::Child{ FirstName = "Zondo", LastName = "Zondini", IsHidden = false ,ProfilePictureUrl = "Content/Images/Children/default-profile-picture.png"},
                                        new db::Child{ FirstName = "Ivo",   LastName = "Sivo",    IsHidden = false ,ProfilePictureUrl = "Content/Images/Children/default-profile-picture.png"}
                                    }
                    }
                    
                }.ToList()
                 .ForEach(user => dataModel.Users.Add(user));
        }
    }
}
