import React, { useState, useEffect } from "react";
import {
  Collapse,
  Card,
  Button,
  Row,
  Col,
  Form,
  Alert,
  ListGroup,
  Modal,
} from "react-bootstrap";
import "./CreateSection";
import useCookies from "react-cookie/cjs/useCookies";
import { API_URL } from "../../Variables";

const CreateSection = ({ setTeams }) => {
  const [cookies] = useCookies();
  const [showCreateTeam, setShowCreateTeam] = useState(false);
  const [showCreatePlayer, setShowCreatePlayer] = useState(false);
  const [playerSuccessMessage, setPlayerSuccessMessage] = useState("");
  const [playerErrorMessage, setPlayerErrorMessage] = useState("");
  const [teamSuccessMessage, setTeamSuccessMessage] = useState("");
  const [teamErrorMessage, setTeamErrorMessage] = useState("");
  const [teamCount, setTeamCount] = useState("");

  const [playerName, setPlayerName] = useState("");
  const [playerNationality, setPlayerNationality] = useState("");
  const [nationalities, setNationalities] = useState([]);
  const [playerPosition, setPlayerPosition] = useState("");
  const [positions, setPositions] = useState([]);
  const [playerGender, setPlayerGender] = useState("");
  const [genders, setGenders] = useState([]);
  const [playerOverall, setPlayerOverall] = useState("");
  const [teamName, setTeamName] = useState("");
  const [teamColor, setTeamColor] = useState("");

  useEffect(() => {
    fetchNationalities();
    fetchPositions();
    fetchGenders();
  }, []);

  const fetchNationalities = async () => {
    try {
      const response = await fetch(`${API_URL}/players/getNationalities`);
      if (response.ok) {
        const data = await response.json();
        setNationalities(data);
      } else {
        console.error("Error fetching nationalities:", response.statusText);
      }
    } catch (error) {
      console.error("Error fetching nationalities:", error);
    }
  };

  const fetchPositions = async () => {
    try {
      const response = await fetch(`${API_URL}/players/getPositions`);
      if (response.ok) {
        const data = await response.json();
        setPositions(data);
      } else {
        console.error("Error fetching positions:", response.statusText);
      }
    } catch (error) {
      console.error("Error fetching positions:", error);
    }
  };

  const fetchGenders = async () => {
    try {
      const response = await fetch(`${API_URL}/players/getGender`);
      if (response.ok) {
        const data = await response.json();
        setGenders(data);
      } else {
        console.error("Error fetching genders:", response.statusText);
      }
    } catch (error) {
      console.error("Error fetching genders:", error);
    }
  };

  const handleCreatePlayer = (e) => {
    e.preventDefault();
    console.log("Player Name:", playerName);
    console.log("Player Nationality:", playerNationality);
    console.log("Player Position:", playerPosition);
    console.log("Player Gender:", playerGender);
    console.log("Player Overall:", playerOverall);
    setPlayerName("");
    setPlayerNationality("");
    setPlayerPosition("");
    setPlayerGender("");
    setPlayerOverall("");
    createPlayer();
  };

  const createPlayer = async () => {
    const playerData = {
      name: playerName,
      nationality: playerNationality,
      position: playerPosition,
      gender: playerGender,
      score: playerOverall,
    };

    try {
      const response = await fetch(`${API_URL}/players/admin/createPlayer`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Authorization": "Bearer " + cookies["token"]
        },
        body: JSON.stringify(playerData),
      });

      if (response.ok) {
        const createdPlayer = await response.json();
        console.log("Player created:", createdPlayer);
        setPlayerSuccessMessage("Player created successfully.");
        setPlayerErrorMessage("");
      } else {
        const errorText = await response.text();
        console.error("Error creating player:", errorText);
        setPlayerSuccessMessage("");
        setPlayerErrorMessage("Error creating player: " + errorText);
      }
    } catch (error) {
      console.error("Error creating player:", error);
      setPlayerSuccessMessage("");
      setPlayerErrorMessage("Error creating player: " + error.message);
    }
  };

  const handleToggleCreateTeam = () => {
    setShowCreateTeam(!showCreateTeam);
    setShowCreatePlayer(false);
  };

  const handleToggleCreatePlayer = () => {
    setShowCreatePlayer(!showCreatePlayer);
    setShowCreateTeam(false);
  };

  const handleCreateTeam = async (event) => {
    event.preventDefault();

    const teamData = {
      name: teamName,
      color: teamColor,
    };

    try {
      const response = await fetch(`${API_URL}/teams/user/addTeam`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Authorization": "Bearer " + cookies["token"]
        },
        body: JSON.stringify(teamData),
      });

      if (response.ok) {
        const createdTeam = await response.json();
        console.log("Team created:", createdTeam);
        setTeams((prevTeams) => [...prevTeams, createdTeam]);
        setTeamName("");
        setTeamColor("");
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
        {playerSuccessMessage && (
          <Alert variant="success" onClose={() => setPlayerSuccessMessage("")}>
            {" "}
            <p>{playerSuccessMessage}</p>
            <hr />
            <div className="d-flex justify-content-end">
              <Button
                onClick={() => setPlayerSuccessMessage("")}
                variant="outline-success"
                size="sm"
              >
                Close
              </Button>
            </div>
          </Alert>
        )}
        {playerErrorMessage && (
          <Alert variant="danger" onClose={() => setPlayerErrorMessage("")}>
            <p>{playerErrorMessage}</p>
            <hr />
            <div className="d-flex justify-content-end">
              <Button
                onClick={() => setPlayerErrorMessage("")}
                variant="outline-danger"
                size="sm"
              >
                Close
              </Button>
            </div>
          </Alert>
        )}

        {teamSuccessMessage && (
          <Alert variant="success" onClose={() => setTeamSuccessMessage("")}>
            <p>{teamSuccessMessage}</p>
            <hr />
            <div className="d-flex justify-content-end">
              <Button
                onClick={() => setTeamSuccessMessage("")}
                variant="outline-success"
                size="sm"
              >
                Close
              </Button>
            </div>
          </Alert>
        )}
        {teamErrorMessage && (
          <Alert variant="danger" onClose={() => setTeamErrorMessage("")}>
            <p>{teamErrorMessage}</p>
            <hr />
            <div className="d-flex justify-content-end">
              <Button
                onClick={() => setTeamErrorMessage("")}
                variant="outline-danger"
                size="sm"
              >
                Close
              </Button>
            </div>
          </Alert>
        )}
        <div className="MyTeams"></div>
        <div className="dashboard-container">
          <Col sm={4}>
            <div className="create-cards-container">
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
                    style={{color: "white", fontWeight: "bold"}}
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
              <Card
                bg="dark"
                text="white"
                className={`create-card ${showCreatePlayer ? "active" : ""}`}
              >
                <Card.Header>
                  <Button
                    variant="text"
                    onClick={handleToggleCreatePlayer}
                    aria-controls="create-player-collapse"
                    aria-expanded={showCreatePlayer}
                    style={{color: "white", fontWeight: "bold"}}
                  >
                    <b>Create Player</b>
                  </Button>
                </Card.Header>
                <Collapse in={showCreatePlayer}>
                  <div id="create-player-collapse">
                    <Card.Body>
                      <Form onSubmit={handleCreatePlayer}>
                        <Form.Group controlId="formPlayerName">
                          <Form.Label>Player Name</Form.Label>
                          <Form.Control
                            type="text"
                            placeholder="Enter player name"
                            value={playerName}
                            onChange={(e) => setPlayerName(e.target.value)}
                          />
                        </Form.Group>
                        <Form.Group controlId="formPlayerNationality">
                          <Form.Label>Nationality</Form.Label>
                          <Form.Control
                            as="select"
                            value={playerNationality}
                            onChange={(e) =>
                              setPlayerNationality(e.target.value)
                            }
                          >
                            <option value="">Select nationality</option>
                            {nationalities.map((nationality) => (
                              <option key={nationality} value={nationality}>
                                {nationality}
                              </option>
                            ))}
                          </Form.Control>
                        </Form.Group>
                        <Form.Group controlId="formPlayerPosition">
                          <Form.Label>Position</Form.Label>
                          <Form.Control
                            as="select"
                            value={playerPosition}
                            onChange={(e) => setPlayerPosition(e.target.value)}
                          >
                            <option value="">Select position</option>
                            {positions.map((position) => (
                              <option key={position} value={position}>
                                {position}
                              </option>
                            ))}
                          </Form.Control>
                        </Form.Group>
                        <Form.Group controlId="formPlayerGender">
                          <Form.Label>Gender</Form.Label>
                          <Form.Control
                            as="select"
                            value={playerGender}
                            onChange={(e) => setPlayerGender(e.target.value)}
                          >
                            <option value="">Select gender</option>
                            {genders.map((gender) => (
                              <option key={gender} value={gender}>
                                {gender}
                              </option>
                            ))}
                          </Form.Control>
                        </Form.Group>
                        <Form.Group controlId="formPlayerOverall">
                          <Form.Label>Overall Score</Form.Label>
                          <Form.Control
                            type="number"
                            placeholder="Enter overall score"
                            value={playerOverall}
                            onChange={(e) => setPlayerOverall(e.target.value)}
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
