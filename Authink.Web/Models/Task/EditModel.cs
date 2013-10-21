using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Authink.Core.Model.Queries;

using core = Authink.Core.Domain.Entities;

namespace Authink.Web.Models.Task
{
    public class EditModel
    {
        public EditModel
        (
            ITaskQueries taskQueries
        )
        {
            this.taskQueries = taskQueries;
        }

        private readonly ITaskQueries taskQueries;

        public string TaskId { get; set; }

        public core::Task.Details Task
        {
            get
            {
                if (_task == null)
                {
                    _task = new Lazy<core::Task.Details>(() => taskQueries.GetSingle_whereId(Convert.ToInt32(this.TaskId)));
                }
                return _task.Value;
            }
        }
        private Lazy<core::Task.Details> _task;
    }
    public class EditDetectItemModel
    {
        public EditDetectItemModel
        (
            ITaskQueries taskQueries,
            IPictureQueries pictureQueries
        )
        {
            this.taskQueries = taskQueries;
            this.pictureQueries = pictureQueries;

        }

        private readonly ITaskQueries taskQueries;
        private readonly IPictureQueries pictureQueries;

        public int TaskId { get; set; }

        [Required(ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description cannot be empty")]
        public string Description { get; set; }

        public core::Task.Details Task
        {
            get
            {
                if (_task == null)
                {
                    _task = new Lazy<core::Task.Details>(() => taskQueries.GetSingle_whereId(this.TaskId));
                }
                return _task.Value;
            }
        }
        private Lazy<core::Task.Details> _task;


        public List<core::Picture.Details> PicturesForTask
        {
            get
            {
                if (_picturesForTask == null)
                {
                    _picturesForTask = new Lazy<List<core::Picture.Details>>(() => pictureQueries.GetAll_forTaskGameplay(this.TaskId).ToList());
                }
                return _picturesForTask.Value;
            }
        }
        private Lazy<List<core::Picture.Details>> _picturesForTask;

        public List<core::Picture.Details> WrongPictures
        {
            get
            {
                if (_wrongPictures == null)
                {
                    _wrongPictures = new Lazy<List<core::Picture.Details>>
                                     (
                                         () => PicturesForTask.Where(picture => !(bool)picture.IsAnswer)
                                                               .Select(picture => picture)
                                                               .ToList()
                                     );
                }
                return _wrongPictures.Value;
            }
        }
        private Lazy<List<core::Picture.Details>> _wrongPictures;

        public core::Picture.Details CorrectPicture
        {
            get
            {
                if (_correctPicture == null)
                {
                    _correctPicture = new Lazy<core::Picture.Details>(() => PicturesForTask.Single(picture => (bool)picture.IsAnswer));
                }
                return _correctPicture.Value;
            }
        }
        private Lazy<core::Picture.Details> _correctPicture;
    }
    public class EditDetectColorsModel
    {
        public EditDetectColorsModel
        (
            ITaskQueries taskQueries,
            IPictureQueries pictureQueries
        )
        {
            this.taskQueries = taskQueries;
            this.pictureQueries = pictureQueries;

        }

        private readonly ITaskQueries taskQueries;
        private readonly IPictureQueries pictureQueries;

        public int TaskId { get; set; }

        [Required(ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description cannot be empty")]
        public string Description { get; set; }

        public core::Task.Details Task
        {
            get
            {
                if (_task == null)
                {
                    _task = new Lazy<core::Task.Details>(() => taskQueries.GetSingle_whereId(this.TaskId));
                }
                return _task.Value;
            }
        }
        private Lazy<core::Task.Details> _task;


        public List<core::Picture.Details> PicturesForTask
        {
            get
            {
                if (_picturesForTask == null)
                {
                    _picturesForTask = new Lazy<List<core::Picture.Details>>(() => pictureQueries.GetAll_forTaskGameplay(this.TaskId).ToList());
                }
                return _picturesForTask.Value;
            }
        }
        private Lazy<List<core::Picture.Details>> _picturesForTask;

        public List<PictureWithColors> PicturesWithColors
        {
            get
            {
                if (_picturesWithColors == null)
                {
                    _picturesWithColors = new Lazy<List<PictureWithColors>>
                                          (() => PicturesForTask.Select(picture =>
                                                                        new
                                                                        {
                                                                            picture = picture,

                                                                            correctColor = pictureQueries.GetAll_colorsForPicture(picture.Id).Single(color => color.IsCorrect),
                                                                            wrongColors = pictureQueries.GetAll_colorsForPicture(picture.Id).Where(color => !color.IsCorrect).Select(color => color).ToList()
                                                                        }
                                                                        ).Select(pictureWithColors => new PictureWithColors(pictureWithColors.picture, pictureWithColors.wrongColors, pictureWithColors.correctColor)).ToList()


                                          );
                }
                return _picturesWithColors.Value;
            }
        }
        private Lazy<List<PictureWithColors>> _picturesWithColors;
    }
    public class EditSimpleTasksWithPicturesModel
    {
        public EditSimpleTasksWithPicturesModel
        (
            ITaskQueries taskQueries,
            IPictureQueries pictureQueries
        )
        {
            this.taskQueries = taskQueries;
            this.pictureQueries = pictureQueries;
        }

        private readonly ITaskQueries taskQueries;
        private readonly IPictureQueries pictureQueries;

        public int Id { get; set; }

        [Required(ErrorMessage = "Description can't be empty")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Name can't be empty")]
        public string Name { get; set; }

        public core::Task.Details Task
        {
            get
            {
                if (_task == null)
                {
                    _task = new Lazy<core::Task.Details>(() => taskQueries.GetSingle_whereId(this.Id));
                }
                return _task.Value;
            }
        }

        private Lazy<core::Task.Details> _task;

        public List<core::Picture.Details> PicturesForTask
        {
            get
            {
                if (_picturesForTask == null)
                {
                    _picturesForTask =
                        new Lazy<List<core::Picture.Details>>(
                            () => pictureQueries.GetAll_forTaskGameplay(this.Id).ToList());
                }
                return _picturesForTask.Value;
            }
        }

        private Lazy<List<core::Picture.Details>> _picturesForTask;
    }

    public class PictureWithColors
    {
        public PictureWithColors()
        {

        }

        public PictureWithColors
        (
            core::Picture.Details picture,
            List<core::Color.Details> wrongColor,
            core::Color.Details correctColor
        )
        {
            this.Picture = picture;
            this.WrongColors = wrongColor;
            this.CorrectColor = correctColor;
        }

        public core::Picture.Details Picture { get; set; }

        public List<core::Color.Details> WrongColors { get; set; }
        public core::Color.Details CorrectColor { get; set; }
    }
}