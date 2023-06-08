import { Table } from "react-bootstrap";
import React, { useState, useEffect } from "react";
import { Button, Row, Col, Alert, ListGroup, Modal, Form } from "react-bootstrap";
import Loading from "../Loading";
import "./ManageSection.css";
import useCookies from "react-cookie/cjs/useCookies";
import EditNameModal from "./EditNameModal";

const ManageSection = () => {
  const [cookies, setCookies] = useCookies();
  const [loading, setLoading] = useState(true);
  const [teams, setTeams] = useState([]);
  const [teamSuccessMessage, setTeamSuccessMessage] = useState("");
  const [teamErrorMessage, setTeamErrorMessage] = useState("");
  const [selectedTeam, setSelectedTeam] = useState(null);
  const [showManageTeamModal, setShowManageTeamModal] = useState(false);
  const [showNameModal, setShowNameModal] = useState(false);
  const [teamName, setTeamName] = useState("");
  

  const fetchTeamsOfUser = (signal) => {
    return fetch(`http://localhost:3000/api/teams/user/getTeams`, {
      headers: {
        'Authorization': "Bearer " + cookies["token"]
      },
      signal,
    }).then((res) => res.json());
  };

  useEffect(() => {
    const controller = new AbortController();
    fetchTeamsOfUser()
      .then((teamsData) => {
        setTeams(teamsData);
        setLoading(false);
      })
      .catch((error) => {
        if (error.name !== "AbortError") {
          setTeams([]);
          throw error;
        }
      });

    return () => controller.abort();
  }, []);

  const handleManageTeamModal = (team) => {
    setSelectedTeam(team);
    setShowManageTeamModal(true);
  };

  const updateTeamName = (teamName, teamId) => {
    return fetch(`/api/teams/user/updateTeamName/${teamId}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        'Authorization': "Bearer " + cookies["token"]
      },
      body: JSON.stringify(teamName), 
    }).then((res) => res.json());
  };
  const handleUpdateTeamNameChange = (teamId) => {
    try {
          updateTeamName(teamName, teamId)
          .then(() => {
            setTeamSuccessMessage("Team name updated successfully")
            setTeamErrorMessage("")
            
          }) 
    } catch (error) {
      console.error("Error creating team:", error);
      setTeamErrorMessage("Can not update")
      setTeamSuccessMessage("");
    }
  }

 

  const handleCloseManageTeamModal = () => {
    setShowManageTeamModal(false);
    setSelectedTeam(null);
  };

  const handleDeleteTeam = async (teamId) => {
    try {
      const response = await fetch(`/api/teams/deleteTeam/${teamId}`, {
        method: "DELETE",
      });

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
                    <td>{team.name}</td>
                  </tr>
                ))}
              </tbody>
            </Table>
        </Row>
      </div>

      {/* Manage Team Modal */}
      {selectedTeam && (
        <Modal
          show={showManageTeamModal}
          onHide={handleCloseManageTeamModal}
          variant="dark"
        >
          <Modal.Header>
            <Modal.Title>Manage Team: {selectedTeam.name}</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Button variant="success">Add a Player</Button>{" "}
            <Button 
              variant="warning" 
              onClick={(e) => setShowNameModal(true)}>
                Edit Name
                </Button>
            <Button
              variant="danger"
              onClick={() => handleDeleteTeam(selectedTeam.id)}
            >
              Delete
            </Button>
          </Modal.Body>
        </Modal>
      )}
      {selectedTeam && (
      <EditNameModal
      setShowNameModal ={setShowNameModal}
    handleUpdateTeamNameChange = {handleUpdateTeamNameChange}
    setTeamName = {setTeamName}
    teamName = {teamName}
    selectedTeam = {selectedTeam}
    showNameModal = {showNameModal}/>)}
    </div>
  );
};

export default ManageSection;
