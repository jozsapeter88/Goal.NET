import React, { useState } from 'react';
import useCookies from "react-cookie/cjs/useCookies";

const TeamCreator = () => {
  const [cookies] = useCookies();
  const [teamName, setTeamName] = useState('');
  const [teamColor, setTeamColor] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    console.log('Team Name:', teamName);
    console.log('Team Color:', teamColor);
    setTeamName('');
    setTeamColor('');
    await createTeam();
  };

  const createTeam = async () => {
    const teamData = {
      name: teamName,
      color: teamColor
    };

    try {
      const response = await fetch(process.env.REACT_APP_API_URL + "/teams/addTeam", {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': "Bearer " + cookies["token"]
        },
        body: JSON.stringify(teamData)
      });

      if (response.ok) {
        const createdTeam = await response.json();
        console.log('Team created:', createdTeam);
      } else {
        console.error('Error creating team:', response.statusText);
      }
    } catch (error) {
      console.error('Error creating team:', error);
    }
  };

  return (
    <div className="container d-flex align-items-center justify-content-center vh-100">
      <div className="card">
        <div className="card-body">
          <h1 className="card-title text-center">Create a Team</h1>
          <form onSubmit={handleSubmit}>
            <div className="mb-3">
              <label htmlFor="teamName" className="form-label">
                Team Name
              </label>
              <input
                type="text"
                className="form-control"
                id="teamName"
                value={teamName}
                onChange={(e) => setTeamName(e.target.value)}
                required
              />
            </div>
            <div className="mb-3">
              <label htmlFor="teamColor" className="form-label">
                Team Color
              </label>
              <input
                type="color"
                className="form-control"
                id="teamColor"
                value={teamColor}
                onChange={(e) => setTeamColor(e.target.value)}
                required
              />
            </div>
            <div className="text-center">
              <button type="submit" className="btn btn-primary">
                Create Team
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default TeamCreator;
