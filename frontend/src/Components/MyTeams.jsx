/*-------------------This component is used in the Dashboard-------------------*/

import React, { useState, useEffect } from "react";
import Loading from "./Loading";
import { Card, Button, Collapse } from "react-bootstrap";
import useCookies from "react-cookie/cjs/useCookies";
import { fetchPlayersOfTeam } from "../Pages/TeamManager/TeamManager";

export default function MyTeams() {
  const [cookies] = useCookies();
  const [teams, setTeams] = useState([]);
  const [teamPlayers, setTeamPlayers] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchTeamsOfUser = async () => {
    try {
      const response = await fetch(process.env.REACT_APP_API_URL + "/teams/user/getTeams", {
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

  const handleToggleDetails = async (teamId) => {
    const playersOfTeam = await fetchPlayersOfTeam(cookies, teamId);
    if(playersOfTeam.status === 200){
      const data = await playersOfTeam.json();
      console.log('data', data )
      setTeamPlayers(data)
    }else if(playersOfTeam.status === 404){
      console.log('no player')
      alert("No players yet!")
    }
    setTeams((prevTeams) =>
      prevTeams.map((team) =>
        team.id === teamId ? { ...team, showDetails: !team.showDetails } : { ...team, showDetails: false }
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
                      {teamPlayers
                        ? teamPlayers.map((player) => (
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
