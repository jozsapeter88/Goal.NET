/*-------------------This component is used in the TeamManager-------------------*/

import React, { useState, useEffect } from "react";
import { Collapse, Card, Button, Col, Form } from "react-bootstrap";
import "./CreateSection";
import useCookies from "react-cookie/cjs/useCookies";

const CreateSection = ({ setTeams }) => {
  const [cookies] = useCookies();
  const [showCreateTeam, setShowCreateTeam] = useState(false);
  const [teamSuccessMessage, setTeamSuccessMessage] = useState("");
  const [teamErrorMessage, setTeamErrorMessage] = useState("");
  const [teamCount, setTeamCount] = useState("");

  const [teamName, setTeamName] = useState("");
  const [teamColor, setTeamColor] = useState("");

  const handleToggleCreateTeam = () => {
    setShowCreateTeam(!showCreateTeam);
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
  };

  return (
    <>
      <div>
        <div>
          <Col sm={4}>
            <div>
              <Card
                bg="dark"
                text="white"
                className={`create-card ${showCreateTeam ? "active" : ""}`}
              >
                <Card.Header>
                  <Button
                    variant="text"
                    onClick={handleToggleCreateTeam}
                    aria-controls="create-team-collapse"
                    aria-expanded={showCreateTeam}
                    style={{ color: "white", fontWeight: "bold" }}
                  >
                    <b>Create Team</b>
                  </Button>
                </Card.Header>
                <Collapse in={showCreateTeam}>
                  <div id="create-team-collapse">
                    <Card.Body>
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
                    </Card.Body>
                  </div>
                </Collapse>
              </Card>
            </div>
          </Col>
        </div>
      </div>
    </>
  );
};

export default CreateSection;
