import React, { useState, useEffect } from "react";
import { Collapse, Card, Button, Row, Col, Form } from "react-bootstrap";
import Loading from "../Loading";

const Dashboard = () => {
  const [loading, setLoading] = useState(true);
  const [teams, setTeams] = useState([]);
  const [showCreateTeam, setShowCreateTeam] = useState(false);

  const fetchTeamData = (teamId, signal) => {
    return fetch(`http://localhost:3000/api/teams/${teamId}`, {
      signal,
    }).then((res) => res.json());
  };

  useEffect(() => {
    const controller = new AbortController();
    const teamIds = [1, 2, 3]; // Replace with the actual team IDs or fetch them from an API
    Promise.all(
      teamIds.map((id) => fetchTeamData(id, controller.signal))
    )
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
  };

  return (
    <div>
      <Row>
        <Col sm={8}>
          {teams.map((team) => (
            <Card
              key={team.id}
              bg="dark"
              text="white"
              style={{ opacity: 0.9, marginBottom: "1rem" }}
            >
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
                      <table>
                        <thead>
                          <tr>
                            <th>Name</th>
                            <th>Position</th>
                            <th>Nationality</th>
                            <th>Overall</th>
                          </tr>
                        </thead>
                        <tbody>
                          {team.allPlayers.map((player) => (
                            <tr key={player.id}>
                              <td>{player.name}</td>
                              <td>{player.position}</td>
                              <td>{player.nationality}</td>
                              <td>{player.score}</td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    )}
                  </Card.Body>
                </div>
              </Collapse>
            </Card>
          ))}
        </Col>
        <Col sm={4}>
        <Card
        bg="dark"
        text="white"
        style={{ opacity: 0.9, marginBottom: "1rem" }}
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
              <Form>
                <Form.Group controlId="teamName">
                  <Form.Label>Name</Form.Label>
                  <Form.Control type="text" required />
                </Form.Group>
                <Form.Group controlId="teamColor">
                  <Form.Label>Color</Form.Label>
                  <Form.Control type="text" required />
                </Form.Group>
              </Form>
            </Card.Body>
          </div>
        </Collapse>
      </Card>
        </Col>
      </Row>
    </div>
  );
};

export default Dashboard;
