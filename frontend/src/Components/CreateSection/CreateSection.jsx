/*-------------------This component is used in the TeamManager-------------------*/

import React, { useState } from "react";
import { Button, Form, Modal } from "react-bootstrap";
import "./CreateSection";
import useCookies from "react-cookie/cjs/useCookies";

const CreateSection = ({ setTeams }) => {
  const [cookies] = useCookies();
  const [showCreateTeam, setShowCreateTeam] = useState(false);
  const [teamSuccessMessage, setTeamSuccessMessage] = useState("");
  const [teamErrorMessage, setTeamErrorMessage] = useState("");
  const [teamCount, setTeamCount] = useState("");
  const [showModal, setShowModal] = useState(false);

  const [teamName, setTeamName] = useState("");
  const [teamColor, setTeamColor] = useState("");

  const handleToggleCreateTeam = () => {
    setShowCreateTeam(!showCreateTeam);
  };

  const handleOpenModal = () => {
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
  };

  const handleCreateTeam = async (event) => {
    event.preventDefault();

    const teamData = {
      name: teamName,
      color: teamColor,
    };

    try {
      const response = await fetch(
        process.env.REACT_APP_API_URL + "/teams/user/addTeamToUser",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + cookies["token"],
          },
          body: JSON.stringify(teamData),
        }
      );

      if (response.ok) {
        console.log(response);
        const createdTeam = await response.json();
        console.log("Team created:", createdTeam);
        setTeams((prevTeams) => [...prevTeams, createdTeam]);
        setTeamName(createdTeam.name);
        setTeamColor(createdTeam.color);
        location.reload();
        setTeamSuccessMessage("Team created successfully.");

        // Check if it's the first team
        if (teamCount === 0) {
          setTeamSuccessMessage("First team created successfully.");
        }

        setTeamCount((prevCount) => prevCount + 1);
        setTeamErrorMessage("");
      } else {
        const errorText = await response.text();
        console.error("Error creating team:", errorText);
        setTeamSuccessMessage("");
        setTeamErrorMessage("Error creating team: " + errorText);
      }
    } catch (error) {
      console.error("Error creating team:", error);
      setTeamSuccessMessage("");
      setTeamErrorMessage("Error creating team: " + error.message);
    }
    handleCloseModal();
  };

  return (
    <>
      <div>
        <Button onClick={handleOpenModal} className="createBtn float-right">
          Create Team
        </Button>

        <Modal show={showModal} onHide={handleCloseModal}>
          <Modal.Header closeButton>
            <Modal.Title>Create Team</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Form onSubmit={handleCreateTeam}>
              <Form.Group controlId="teamName">
                <Form.Label>Name</Form.Label>
                <Form.Control
                  type="text"
                  required
                  value={teamName}
                  onChange={(e) => setTeamName(e.target.value)}
                />
              </Form.Group>
              <Form.Group controlId="teamColor">
                <Form.Label>Color</Form.Label>
                <Form.Control
                  type="text"
                  required
                  value={teamColor}
                  onChange={(e) => setTeamColor(e.target.value)}
                />
              </Form.Group>
              <Button variant="primary" type="submit">
                Create
              </Button>
            </Form>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={handleCloseModal}>
              Close
            </Button>
          </Modal.Footer>
        </Modal>
      </div>
    </>
  );
};

export default CreateSection;
