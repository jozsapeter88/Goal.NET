/*-------------------This component is used in the TeamManager-------------------*/

import React, { useState, useEffect } from "react";
import { Row, Table } from "react-bootstrap";
import Loading from "../Loading";
import "./ManageSection.css";
import useCookies from "react-cookie/cjs/useCookies";
import EditNameModal from "../EditNameModal";
import ManageTeamModal from "../ManageTeamModal";

const ManageSection = ({
  teams,
  setTeams,
  loading,
  loadingPlayers,
  players,
}) => {
  const [cookies] = useCookies();
  const [teamSuccessMessage, setTeamSuccessMessage] = useState("");
  const [teamErrorMessage, setTeamErrorMessage] = useState("");
  const [selectedTeam, setSelectedTeam] = useState(null);
  const [showManageTeamModal, setShowManageTeamModal] = useState(false);
  const [showNameModal, setShowNameModal] = useState(false);
  const [teamName, setTeamName] = useState("");
  const [showTeamList, setShowTeamList] = useState(false);

  const handleManageTeamModal = (team) => {
    setSelectedTeam(team);
    setTeamName(team.name);
    setShowManageTeamModal(true);
  };

  const updateTeamName = (teamName, teamId) => {
    return fetch(process.env.REACT_APP_API_URL + `/teams/user/updateTeamName/${teamId}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + cookies["token"],
      },
      body: JSON.stringify(teamName),
    }).then((res) => res.json());
  };

  const handleUpdateTeamNameChange = (teamId) => {
    try {
      updateTeamName(teamName, teamId).then(() => {
        setTeamSuccessMessage("Team name updated successfully");
        setTeamErrorMessage("");
        console.log(teamSuccessMessage);
      });
    } catch (error) {
      console.error("Error updating team name:", error);
      setTeamErrorMessage("Can not update team name");
      setTeamSuccessMessage("");
      console.log(teamErrorMessage);
    }
  };

  const handleCloseManageTeamModal = () => {
    setShowManageTeamModal(false);

    setSelectedTeam(null);
    setShowTeamList(false);
  };

  const handleDeleteTeam = async (teamId) => {
    try {
      const response = await fetch(
        process.env.REACT_APP_API_URL +`/teams/user/deleteTeam/${teamId}`,
        {
          method: "DELETE",
          headers: {
            Authorization: "Bearer " + cookies["token"],
          },
        }
      );

      if (response.ok) {
        setTeams((prevTeams) => prevTeams.filter((team) => team.id !== teamId));
        setTeamSuccessMessage("Team deleted successfully.");
        setTeamErrorMessage("");
      } else {
        const errorText = await response.text();
        console.error("Error deleting team:", errorText);
        setTeamSuccessMessage("");
        setTeamErrorMessage("Error deleting team: " + errorText);
      }
    } catch (error) {
      console.error("Error deleting team:", error);
      setTeamSuccessMessage("");
      setTeamErrorMessage("Error deleting team: " + error.message);
    }
  };

  if (loading || loadingPlayers) {
    return <Loading />;
  }

  return (
    <div>
      <div className="dashboard-container">
        <Row>
          <Table striped bordered hover variant="dark">
            <thead>
              <tr>
                <th>Manage Your Team</th>
              </tr>
            </thead>
            <tbody>
              {teams.map((team) => (
                <tr key={team.id} onClick={() => handleManageTeamModal(team)}>
                  <td className="team-name">{team.name}</td>
                </tr>
              ))}
            </tbody>
          </Table>
        </Row>
      </div>
      {/* Manage Team Modal */}
      {selectedTeam && (
        <ManageTeamModal
          showManageTeamModal={showManageTeamModal}
          handleCloseManageTeamModal={handleCloseManageTeamModal}
          selectedTeam={selectedTeam}
          setShowNameModal={setShowNameModal}
          handleDeleteTeam={handleDeleteTeam}
          setShowTeamList={setShowTeamList}
          showTeamList={showTeamList}
          players={players}
          loadingPlayers={loadingPlayers}
        />
      )}
      {/* Name Modal */}
      {selectedTeam && (
        <EditNameModal
          setShowNameModal={setShowNameModal}
          handleUpdateTeamNameChange={handleUpdateTeamNameChange}
          setTeamName={setTeamName}
          teamName={teamName}
          selectedTeam={selectedTeam}
          showNameModal={showNameModal}
        />
      )}
    </div>
  );
};

export default ManageSection;
