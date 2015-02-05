﻿namespace VsSolutionPersister
{
    public class SolutionItem
    {
        public string Name 
        { 
            get
            {
                return string.Format("{0}___{1}", GitBranchName, RemoteSolutionStartPage.Replace('/', '_'));
            } 
        }

        public string RemoteSolutionStartPage { get; set; }

        public string GitBranchName { get; set; }
    }
}
