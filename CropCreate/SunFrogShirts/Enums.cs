using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunFrogShirts
{
    public class Enums
    {
        public enum Category : int
        {
            Automotive = 52,
            Birth_Years = 76,
            Drinking = 78,
            Events = 25,
            Faith = 26,
            Fitness = 61,
            Funny = 19,
            Gamer = 13,
            Geek_Tech = 24,
            Hobby = 82,
            Holidays = 35,
            Jobs = 79,
            LifeStyle = 43,
            Movies = 12,
            Music = 71,
            Names = 75,
            Outdoor = 81,
            Pets = 62,
            Political = 17,
            Sports = 27,
            States = 77,
            TV_Shows = 34,
            Zombies = 11
        }

        public enum UploadType : int
        {
            Shirts,
            Leggings,
            Mugs,
            Posters,
            Cavans,
            Hat,
            Trucker_Cap
        }

        public enum ShirtsType:int
        {
            Guys,
            Ladies,
            Youth,
            Hoodie,
            Sweatshirt,
            Guys_VNeck,
            Ladies_VNeck,
            Unisex_Tank_Tops,
            Unisex_Long_Sleeve,
        }
    }
}
