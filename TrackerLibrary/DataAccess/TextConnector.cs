﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;


namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";
        private const string PeopleFile = "PersonModels.csv";
        private const string TeamFile = "TeamModels.csv";
        private const string TournamentFile = "TournamentModels.csv";

        public PersonModel CreatePerson(PersonModel model)
        {
            var people = PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            int currentId = 1;

            if (people.Count() > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            people.Add(model);

            people.SaveToPeopleFile(PeopleFile);

            return model;
        }

        public PrizeModel CreatePrize(PrizeModel model)
        {
            var prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            int currentId = 1;

            if (prizes.Count() > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            prizes.Add(model);

            prizes.SaveToPrizeFile(PrizesFile);

            return model;
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            var teams = TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(PeopleFile);

            int currentId = 1;

            if (teams.Count() > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            teams.Add(model);

            teams.SaveToTeamFile(TeamFile);

            return model;
        }

        public void CreateTournament(TournamentModel model)
        {
            var tournaments = TournamentFile.FullFilePath().LoadFile().ConvertToTournamentModels(TeamFile, PeopleFile, PrizesFile);
            int currentId = 1;

            if (tournaments.Count() > 0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            tournaments.Add(model);

            tournaments.SaveToTournamentFile(TournamentFile);
        }

        public List<PersonModel> GetPerson_All()
        {
            return PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        public List<TeamModel> GetTeam_All()
        {
            return TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(PeopleFile);
        }
    }
}
