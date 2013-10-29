using System;
using System.Collections.Generic;
using System.Linq;

using Authink.Core.Model.Queries;

using ent = Authink.Core.Domain.Entities;

namespace Authink.Web.Models.Picture
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

        public int TaskId { get; set; }

        public ent::Task.LongDetails Task
        {
            get
            {
                if (_task == null)
                {
                    _task = new Lazy<ent::Task.LongDetails>(() => taskQueries.GetSingle_longDetails_whereId(this.TaskId));
                }
                return _task.Value;
            }
        }
        private Lazy<ent::Task.LongDetails> _task;
    }
    public class EditSimpleModel
    {
        public int TaskId    { get; set; }
        public int PictureId { get; set; }
    }

    public class EditWithColorsModel
    {
        public EditWithColorsModel
        (
            IPictureQueries pictureQueries,
            ITestQueries testQueries
        )
        {
            this.pictureQueries = pictureQueries;
            this.testQueries = testQueries;
        }

        private readonly IPictureQueries pictureQueries;
        private readonly ITestQueries testQueries;

        public int PictureId { get; set; }
        public int TaskId { get; set; }

        public List<Color> WrongColorsToEdit { get; set; }
        public Color CorrectColorToEdit { get; set; }

        public ent::Picture Picture
        {
            get
            {
                if (_picture == null)
                {
                    _picture = new Lazy<ent::Picture>(() => pictureQueries.GetSingle_whereId(this.PictureId));
                }
                return _picture.Value;
            }
        }
        private Lazy<ent::Picture> _picture;

        public List<ent::Color.Details> Colors
        {
            get
            {
                if (_colors == null)
                {
                    _colors = new Lazy<List<ent::Color.Details>>(() => pictureQueries.GetAll_colorsForPicture(this.PictureId).ToList());
                }
                return _colors.Value;
            }
        }
        private Lazy<List<ent::Color.Details>> _colors;

        public ent::Test.ShortDetails Test
        {
            get
            {
                if (_test == null)
                {
                    _test = new Lazy<ent::Test.ShortDetails>(() => testQueries.GetTest_thatContainsTask(this.TaskId));
                }
                return _test.Value;
            }
        }
        private Lazy<ent::Test.ShortDetails> _test;
    }
    public class Color
    {
        public Color()
        {

        }
        public Color
        (
            int id,
            string value
        )
        {
            this.Id = id;
            this.Value = value;
        }

        public int Id { get; set; }
        public string Value { get; set; }
    }
}