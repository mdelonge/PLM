﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace PLM
{
    public class UserGameSession
    {
        public ApplicationUser currentUser { get; set; }
        public int Score { get; set; }
        public int currentGuess { get; set; }
        public int iteratedGuess { get; set; }
        public Module currentModule { get; set; }
        public List<Picture> Pictures { get; set; }
        public List<AnsPicIndex> PictureIndicies { get; set; }
        public int numAnswers { get; set; }
        public int numQuestions { get; set; }
        public int time { get; set; }

        public UserGameSession()
        {
            currentModule = new Module();
            Pictures = new List<Picture>();
            PictureIndicies = new List<AnsPicIndex>();
        }
    }
}