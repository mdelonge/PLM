﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PLM
{
    public static class ShuffleExtension
    {
        //this randomization method, based on the Fisher-Yates shuffle, was taken from http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp
        private static Random rand = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public static class TopTenScore
    {
        public static List<Score> GetTopTenScores(int moduleID, string userID)
        {
            if (userID == null)
            {
                //The current Guest Account ID
                // TODO - Set up a txt file that this can be read from, or find another way of selecting the guest account if not logged in
                userID = "5b853c48-424f-455e-b731-f24e102cdc6d";
            }

            ApplicationDbContext db = new ApplicationDbContext();
            List<Score> scores = db.Scores.ToList();
            scores.Where(x => x.Module.ModuleID == moduleID);
            scores.Where(y => y.User.Id == userID);
            scores.OrderBy(x => (x.TotalAnswers / x.CorrectAnswers)).ToList();
            return((List<Score>)scores.Take(10));
        }
    }

    public static class IdentityExtensions
    {
        public static string GetFirstName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FirstName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetLastName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("LastName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetLocation(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ProfilePicture");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }



        public static string GetInstution(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Instution");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }

    public static class PagingExtensions
    {
        //used by LINQ to SQL
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        //used by LINQ
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

    }

    //Taken from http://stackoverflow.com/questions/3216496/c-sharp-how-to-determine-if-a-number-is-a-multiple-of-another
    public static class MathExtensions
    {
        /// <summary>
        /// Check if this number is evenly divisible by another number
        /// </summary>
        /// <param name="dividend">The number this method is being applied to</param>
        /// <param name="divisor">The number to divide by</param>
        /// <returns>bool</returns>
        public static bool IsDivisible(this int dividend, int divisor)
        {
            return (dividend % divisor) == 0;
        }
    }
}