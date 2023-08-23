/*-------------------This component is used in the Dashboard-------------------*/

import React, { useState, useEffect } from "react";
import Loading from "./Loading";
import { Card, Button, Collapse } from "react-bootstrap";
import useCookies from "react-cookie/cjs/useCookies";
import { API_URL } from "../Variables";

export default function MyTeams() {
  const [cookies] = useCookies();
  const [teams, setTeams] = useState([]);
  const [players, setPlayers] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchTeamsOfUser = async () => {
    try {
      const response = await fetch(`${API_URL}/teams/user/getTeams`, {
        headers: {
          Authorization: "Bearer " + cookies["token"],
        },
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
  const fetchPlayersOfUsersTeam = async (teamId) => {
    try {
      const response = await fetch(
        `${API_URL}/teams/user/getPlayersOfTeam/${teamId}`
      );
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
    fetchTeamsOfUser()
      .then((teamsData) => {
        setTeams(teamsData);
        setLoading(false);
      })
      .catch((error) => {
        setTeams([teams]);
        console.error("Error fetching teams:", error);
      });
  }, []);

  if (loading) {
    return <Loading />;
  }

  if (teams.length === 0) {
    return (
      <Card bg="secondary" style={{ color: "white" }} className="notfound-card">
        <Card.Body>
          <Card.Title>No teams found</Card.Title>
          <Card.Text>Go to the Team Manager to create a team</Card.Text>
        </Card.Body>
      </Card>
    );
  }

  const handleToggleDetails = (teamId) => {
    fetchPlayersOfUsersTeam(teamId);
    setTeams((prevTeams) =>
      prevTeams.map((team) =>
        team.id === teamId ? { ...team, showDetails: !team.showDetails } : team
      )
    );
  };

  return (
    <div>
      {teams.map((team) => (
        <Card key={team.id} bg="dark" text="white" className="team-card">
          <Card.Header>
            <Button
              variant="text"
              onClick={() => handleToggleDetails(team.id)}
              aria-controls={`details-collapse-${team.id}`}
              aria-expanded={team.showDetails}
              className="team-card-title"
              style={{ color: "white", fontWeight: "bold" }}
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
    </div>
  );
}
