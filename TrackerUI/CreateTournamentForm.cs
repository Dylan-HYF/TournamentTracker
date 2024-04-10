using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_All();
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes = new List<PrizeModel>();


        public CreateTournamentForm()
        {
            InitializeComponent();
            InitializeLists();
        }

        private void InitializeLists()
        {
            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = null;
            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";

            prizeListBox.DataSource = null;
            prizeListBox.DataSource = selectedPrizes;
            prizeListBox.DisplayMember = "PlaceName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            if (selectTeamDropDown.SelectedItem == null)
            {
                return;
            }
            var p = (TeamModel)selectTeamDropDown.SelectedItem;

            availableTeams.Remove(p);
            selectedTeams.Add(p);

            InitializeLists();
        }

        private void removeSelectedPlayerButton_Click(object sender, EventArgs e)
        {
            if (tournamentTeamsListBox.SelectedItem == null)
            {
                return;
            }
            var p = (TeamModel)tournamentTeamsListBox.SelectedItem;

            availableTeams.Add(p);
            selectedTeams.Remove(p);

            InitializeLists();
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            CreatePrizeForm frm = new CreatePrizeForm(this);
            frm.Show();
        }

        public void PrizeComplete(PrizeModel model)
        {
            selectedPrizes.Add(model);
            InitializeLists();
        }

        public void TeamComplete(TeamModel model)
        {
            selectedTeams.Add(model);
            InitializeLists();
        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var frm = new CreateTeamForm(this);
            frm.Show();
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            if (prizeListBox.SelectedItem == null)
            {
                return;
            }
            var p = (PrizeModel)prizeListBox.SelectedItem;

            selectedPrizes.Remove(p);

            InitializeLists();
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            bool feeAcceptable = decimal.TryParse(entryFeeValue.Text, out decimal fee);

            if (!feeAcceptable)
            {
                MessageBox.Show("Invalid entry fee.", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var tm = new TournamentModel();

            tm.TournamentName = tournamentNameValue.Text;
            tm.EntryFee = fee;

            tm.Prizes = selectedPrizes;
            tm.EnteredTeams = selectedTeams;



            GlobalConfig.Connection.CreateTournament(tm);
        }
    }
}
