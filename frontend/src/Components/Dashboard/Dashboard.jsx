import React, { useState, useEffect } from "react";
import { Collapse, Card, Button, Row, Col, Form } from "react-bootstrap";
import Loading from "../Loading";
import "./Dashboard.css";

const Dashboard = () => {
  const [loading, setLoading] = useState(true);
  const [teams, setTeams] = useState([]);
  const [showCreateTeam, setShowCreateTeam] = useState(false);
  const [showCreatePlayer, setShowCreatePlayer] = useState(false);

  const fetchTeamsOfUser = (userId, signal) => {
    return fetch(`http://localhost:3000/api/teams/user/${userId}`, {
      signal,
    }).then((res) => res.json());
  };

  useEffect(() => {
    const controller = new AbortController();
    const userId = 1;
    fetchTeamsOfUser(userId)
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

  if (loading) {
    return <Loading />;
  }

  const handleToggleDetails = (teamId) => {
    setTeams((prevTeams) =>
      prevTeams.map((team) =>
        team.id === teamId ? { ...team, showDetails: !team.showDetails } : team
      )
    );
  };

  const handleToggleCreateTeam = () => {
    setShowCreateTeam(!showCreateTeam);
    setShowCreatePlayer(false);
  };

  const handleToggleCreatePlayer = () => {
    setShowCreatePlayer(!showCreatePlayer);
    setShowCreateTeam(false);
  };

  const handleCreateTeam = (event) => {
    event.preventDefault();
  };

  const handleCreatePlayer = (event) => {
    event.preventDefault();
  };

  return (
    <div>
      <div className="MyTeams">
        <h1>My Teams</h1>
      </div>
      <div className="dashboard-container">
        <Row>
          <Col sm={8}>
            {teams.map((team) => (
              <Card key={team.id} bg="dark" text="white" className="team-card">
                <Card.Header>
                  <Button
                    variant="link"
                    onClick={() => handleToggleDetails(team.id)}
                    aria-controls={`details-collapse-${team.id}`}
                    aria-expanded={team.showDetails}
                  >
                    {team.name}
                  </Button>
                </Card.Header>
                <Collapse in={team.showDetails}>
                  <div id={`details-collapse-${team.id}`}>
                    <Card.Body>
                      {team.showDetails && (
                        <table className="player-table">
                          <thead>
                            <tr>
                              <th>Name</th>
                              <th>Position</th>
                              <th>Nationality</th>
                              <th>Overall</th>
                            </tr>
                          </thead>
                          <tbody>
                            {team.allPlayers
                              ? team.allPlayers.map((player) => (
                                  <tr key={player.id}>
                                    <td>{player.name}</td>
                                    <td>{player.position}</td>
                                    <td>{player.nationality}</td>
                                    <td>{player.score}</td>
                                  </tr>
                                ))
                              : console.log("no players yet")}
                          </tbody>
                          <Button
                            variant="secondary"
                            size="sm"
                            className="ml-2"
                          >
                            Add Player
                          </Button>
                        </table>
                      )}
                    </Card.Body>
                  </div>
                </Collapse>
              </Card>
            ))}
          </Col>
          <Col sm={4}>
            <div className="create-cards-container">
              <Card
                bg="dark"
                text="white"
                className={`create-card ${showCreateTeam ? "active" : ""}`}
              >
                <Card.Header>
                  <Button
                    variant="link"
                    onClick={handleToggleCreateTeam}
                    aria-controls="create-team-collapse"
                    aria-expanded={showCreateTeam}
                  >
                    Create New Team
                  </Button>
                </Card.Header>
                <Collapse in={showCreateTeam}>
                  <div id="create-team-collapse">
                    <Card.Body>
                      <Form onSubmit={handleCreateTeam}>
                        <Form.Group controlId="teamName">
                          <Form.Label>Name</Form.Label>
                          <Form.Control type="text" required />
                        </Form.Group>
                        <Form.Group controlId="teamColor">
                          <Form.Label>Color</Form.Label>
                          <Form.Control type="text" required />
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
                    variant="link"
                    onClick={handleToggleCreatePlayer}
                    aria-controls="create-player-collapse"
                    aria-expanded={showCreatePlayer}
                  >
                    Create New Player
                  </Button>
                </Card.Header>
                <Collapse in={showCreatePlayer}>
                  <div id="create-player-collapse">
                    <Card.Body>
                      <Form onSubmit={handleCreatePlayer}>
                        <Form.Group controlId="playerName">
                          <Form.Label>Name</Form.Label>
                          <Form.Control type="text" required />
                        </Form.Group>
                        <Form.Group controlId="playerPosition">
                          <Form.Label>Position</Form.Label>
                          <Form.Control type="text" required />
                        </Form.Group>
                        <Form.Group controlId="playerNationality">
                          <Form.Label>Nationality</Form.Label>
                          <Form.Control type="text" required />
                        </Form.Group>
                        <Form.Group controlId="playerScore">
                          <Form.Label>Overall</Form.Label>
                          <Form.Control type="number" required />
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
        </Row>
      </div>
    </div>
  );
};

export default Dashboard;
