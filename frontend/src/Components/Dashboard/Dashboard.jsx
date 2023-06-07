import React, { useState, useEffect } from "react";
import { Collapse, Card, Button, Row, Col, Table, Modal } from "react-bootstrap";
import Loading from "../Loading";
import "./Dashboard.css";
import useCookies from "react-cookie/cjs/useCookies";

import { useParams } from "react-router-dom";

const Dashboard = () => {
  const [cookies, setCookies] = useCookies();
  const [loading, setLoading] = useState(true);
  const [teams, setTeams] = useState([]);
  const [players, setPlayers] = useState([]);
  const [expandedMatchId, setExpandedMatchId] = useState(null);
  const [showDetailsModal, setShowDetailsModal] = useState(false);
  const [selectedMatchDetails, setSelectedMatchDetails] = useState("");
  //const { userId } = useParams();
  const userId = 1;
  console.log(players.length);

  const fetchTeamsOfUser = async (userId) => {
    try {
      const response = await fetch(`/api/teams/user/${userId}`, {
        headers: {
          'Authorization': "Bearer " + cookies["token"]
        }
      });
      if (response.ok) {
        const data = await response.json();
        return data;
      } else {
        console.error("Error fetching teams:", response.statusText);
      }
    } catch (error) {
      console.error("Error fetching teams:", error);
    }
    return [];
  };
  const fetchPlayersOfUsersTeam = async(userId, teamId) => {
    try {
      const response = await fetch(`/api/teams/user/${userId}/${teamId}`);
      if (response.ok) {
        const data = await response.json();
        setPlayers(data);
      } else {
        console.error("Error fetching players:", response.statusText);
      }
    } catch (error) {
      console.error("Error fetching players:", error);
    }
    return [];
  };

  useEffect(() => {
    const userId = 1;
    fetchTeamsOfUser(userId)
      .then((teamsData) => {
        setTeams(teamsData);
        setLoading(false);
      })
      .catch((error) => {
        setTeams([]);
        console.error("Error fetching teams:", error);
      });
  }, []);

  if (loading) {
    return <Loading />;
  }

  if (teams.length === 0) {
    return (
      <Card bg="secondary" style={{color: "white"}}>
        <Card.Body>
          <Card.Title>No teams found</Card.Title>
          <Card.Text>
            Go to the Team Manager to create a team
          </Card.Text>
        </Card.Body>
      </Card>
    );
  }

  const handleToggleDetails = (teamId) => {
    fetchPlayersOfUsersTeam(userId, teamId)
    setTeams((prevTeams) =>
      prevTeams.map((team) =>
        team.id === teamId ? { ...team, showDetails: !team.showDetails } : team
      )
    );
  };

  const handleMatchClick = (matchId, matchDetails) => {
    setExpandedMatchId((prevMatchId) => (prevMatchId === matchId ? null : matchId));
    setSelectedMatchDetails(matchDetails);
    setShowDetailsModal(true);
  };

  const handleCloseModal = () => {
    setShowDetailsModal(false);
  };

  /*-------------------Hardcoded match history data-------------------*/
  const matchHistory = [
    {
      id: 1,
      team1: "Real Madrid",
      team2: "Barcelona",
      score: "2-1",
      details: "Match details for Real Madrid vs Barcelona...",
    },
    {
      id: 2,
      team1: "Liverpool",
      team2: "Manchester City",
      score: "0-0",
      details: "Match details for Liverpool vs Manchester City...",
    },
    {
      id: 3,
      team1: "Bayern Munich",
      team2: "Paris Saint-Germain",
      score: "3-2",
      details: "Match details for Bayern Munich vs Paris Saint-Germain...",
    },
    {
      id: 4,
      team1: "Manchester United",
      team2: "Chelsea",
      score: "1-1",
      details: "Match details for Manchester United vs Chelsea...",
    },
    {
      id: 5,
      team1: "Juventus",
      team2: "AC Milan",
      score: "2-0",
      details: "Match details for Juventus vs AC Milan...",
    },
    {
      id: 6,
      team1: "Borussia Dortmund",
      team2: "RB Leipzig",
      score: "2-2",
      details: "Match details for Borussia Dortmund vs RB Leipzig...",
    },
    {
      id: 7,
      team1: "Arsenal",
      team2: "Tottenham Hotspur",
      score: "1-0",
      details: "Match details for Arsenal vs Tottenham Hotspur...",
    },
    {
      id: 8,
      team1: "Inter Milan",
      team2: "Lazio",
      score: "3-1",
      details: "Match details for Inter Milan vs Lazio...",
    },
    {
      id: 9,
      team1: "Atletico Madrid",
      team2: "Sevilla",
      score: "2-2",
      details: "Match details for Atletico Madrid vs Sevilla...",
    },
    {
      id: 10,
      team1: "Ajax",
      team2: "PSV Eindhoven",
      score: "2-1",
      details: "Match details for Ajax vs PSV Eindhoven...",
    },
  ];

  return (
    <div>
      <div className="MyTeams">
        <h1 style={{ color: "white" }}>My Teams</h1>
      </div>
      <div className="dashboard-container">
        <Row>
          <Col sm={4}>
            {teams.map((team) => (
              <Card key={team.id} bg="dark" text="white" className="team-card">
                <Card.Header>
                  <Button
                    variant="link"
                    onClick={() => handleToggleDetails(team.id)}
                    aria-controls={`details-collapse-${team.id}`}
                    aria-expanded={team.showDetails}
                    className="team-card-title"
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
                            {players
                              ? players.map((player) => (
                                  <tr key={player.id}>
                                    <td>{player.name}</td>
                                    <td>{player.position}</td>
                                    <td>{player.nationality}</td>
                                    <td>{player.score}</td>
                                  </tr>
                                ))
                              : null}
                          </tbody>
                        </table>
                      )}
                    </Card.Body>
                  </div>
                </Collapse>
              </Card>
            ))}
          </Col>
          <Col sm={4} className="mb-4">
            <Card bg="dark" text="white">
              <Card.Body>
                <h4>Previous Match History</h4>
                <Table striped bordered hover variant="dark">
                  <thead>
                    <tr>
                      <th>Team 1</th>
                      <th>Team 2</th>
                      <th>Score</th>
                      <th></th>
                    </tr>
                  </thead>
                  <tbody>
                    {matchHistory.map((match) => (
                      <React.Fragment key={match.id}>
                        <tr onClick={() => handleMatchClick(match.id, match.details)}>
                          <td>{match.team1}</td>
                          <td>{match.team2}</td>
                          <td>{match.score}</td>
                          <td>
                          </td>
                        </tr>
                      </React.Fragment>
                    ))}
                  </tbody>
                </Table>
              </Card.Body>
            </Card>
          </Col>
        </Row>
      </div>
      <Modal show={showDetailsModal} onHide={handleCloseModal} size="lg" centered>
        <Modal.Header>
          <Modal.Title>Match Details</Modal.Title>
        </Modal.Header>
        <Modal.Body>{selectedMatchDetails}</Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseModal}>
            Close
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
};

export default Dashboard;
