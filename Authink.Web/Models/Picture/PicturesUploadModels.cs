using System;
using System.Collections.Generic;
using System.Linq;

using buru = Authink.Core.Domain.Rules;

namespace Authink.Web.Models.Picture
{
    public class UploadPictures_DetectColorsModel
    {
        public int TaskDifficulty { get; set; }
        public string ErrorMessage { get; set; }

        public List<ColorData> Colors { get; set; }

        private static IDictionary<int, NumberOfPicturesAndColorsMapping> KnownNumberOfPicturesAndWrongColorsMappings = new Dictionary<int, NumberOfPicturesAndColorsMapping>
        {
            {1,new NumberOfPicturesAndColorsMapping(1, 1)}, {2, new NumberOfPicturesAndColorsMapping(2, 2)}, {3, new NumberOfPicturesAndColorsMapping(3, 2)},
            {4,new NumberOfPicturesAndColorsMapping(2, 3)}, {5, new NumberOfPicturesAndColorsMapping(2, 4)}, {6, new NumberOfPicturesAndColorsMapping(2, 5)}, 
        };

        public NumberOfPicturesAndColorsMapping NumberOfPicturesAndWrongColorsMapping
        {
            get
            {
                if (_numberOfWrongColors == null)
                {
                    _numberOfWrongColors = new Lazy<NumberOfPicturesAndColorsMapping>(() => KnownNumberOfPicturesAndWrongColorsMappings[this.TaskDifficulty]);
                }
                return _numberOfWrongColors.Value;
            }
        }
        private Lazy<NumberOfPicturesAndColorsMapping> _numberOfWrongColors;
    }

    public class ColorData
    {
        public string CorrectColor { get; set; }

        public List<string> WrongColors { get; set; }
    }
    public class NumberOfPicturesAndColorsMapping
    {
        public NumberOfPicturesAndColorsMapping
        (
            int numberOfPictures,
            int numberOfWrongColorsPerPicture
        )
        {
            this.NumberOfPictures = numberOfPictures;
            this.NumberOfWrongColorsPerPicture = numberOfWrongColorsPerPicture;
        }
        public int NumberOfPictures { get; set; }
        public int NumberOfWrongColorsPerPicture { get; set; }
    }

    public class UploadPictures_SimpleTasksWithPictures
    {
        private static IDictionary<string, List<KnownSpecialDifficultyMapping>> KnownSpecialDifficultyMappings = new Dictionary<string, List<KnownSpecialDifficultyMapping>>
        {
            {buru::Task.Keys.DetectDifferentItems, new List<KnownSpecialDifficultyMapping>{ new KnownSpecialDifficultyMapping(1, 2), new KnownSpecialDifficultyMapping(2, 3), new KnownSpecialDifficultyMapping(3, 4), new KnownSpecialDifficultyMapping(4, 5), new KnownSpecialDifficultyMapping(5, 2)} }
        };

        public int TaskDifficulty { get; set; }
        public string TaskKey { get; set; }
        public string ErrorMessage { get; set; }

        public int NumberOfPictures
        {
            get
            {
                if (_numberOfPictures == null)
                {
                    _numberOfPictures = new Lazy<int>
                     (() => KnownSpecialDifficultyMappings.ContainsKey(TaskKey)
                            ? KnownSpecialDifficultyMappings[TaskKey].Single(pair => pair.TaskDifficulty == this.TaskDifficulty).NumberOfPictures
                            : this.TaskDifficulty
                     );
                }
                return _numberOfPictures.Value;
            }
        }
        private Lazy<int> _numberOfPictures;
    }
    internal class KnownSpecialDifficultyMapping
    {
        public KnownSpecialDifficultyMapping
        (
            int taskDifficulty,
            int numberOfPictures
        )
        {
            this.TaskDifficulty   = taskDifficulty;
            this.NumberOfPictures = numberOfPictures;
        }

        public int TaskDifficulty   { get; private set; }
        public int NumberOfPictures { get; private set; }
    }

    public class UploadPictures_DetectItemModel
    {
        public int    TaskDifficulty { get; set; }
        public string ErrorMessage   { get; set; }
    }
    public class UploadPictures_DetectSameItemsModel
    {
        public int    TaskDifficulty { get; set; }
        public string ErrorMessage   { get; set; }
    }
    public class UploadPictures_OrderBySizeModel
    {
        public int    TaskDifficulty { get; set; }
        public string ErrorMessage   { get; set; }
    }


}