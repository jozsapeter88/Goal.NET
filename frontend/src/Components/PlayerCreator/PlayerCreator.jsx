import React, { useState, useEffect } from 'react';

const PlayerCreator = () => {
  const [playerName, setPlayerName] = useState('');
  const [playerNationality, setPlayerNationality] = useState('');
  const [nationalities, setNationalities] = useState([]);
  const [playerPosition, setPlayerPosition] = useState('');
  const [positions, setPositions] = useState([]);

  useEffect(() => {
    fetchNationalities();
    fetchPositions();
  }, []);

  const fetchNationalities = async () => {
    try {
      const response = await fetch('/api/players/getNationalities');
      if (response.ok) {
        const data = await response.json();
        setNationalities(data);
      } else {
        console.error('Error fetching nationalities:', response.statusText);
      }
    } catch (error) {
      console.error('Error fetching nationalities:', error);
    }
  };

  const fetchPositions = async () => {
    try {
      const response = await fetch('/api/players/getPositions');
      if (response.ok) {
        const data = await response.json();
        setPositions(data);
      } else {
        console.error('Error fetching positions:', response.statusText);
      }
    } catch (error) {
      console.error('Error fetching positions:', error);
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Player Name:', playerName);
    console.log('Player Nationality:', playerNationality);
    console.log('Player Position:', playerPosition);
    setPlayerName('');
    setPlayerNationality('');
    setPlayerPosition('');
    createPlayer();
  };

  const createPlayer = async () => {
    const playerData = {
      name: playerName,
      nationality: playerNationality,
      position: playerPosition
    };

    try {
      const response = await fetch('/api/players/admin/createPlayer', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(playerData)
      });

      if (response.ok) {
        const createdPlayer = await response.json();
        console.log('Player created:', createdPlayer);
      } else {
        console.error('Error creating player:', response.statusText);
      }
    } catch (error) {
      console.error('Error creating player:', error);
    }
  };

  return (
    <div className="container d-flex align-items-center justify-content-center vh-100">
      <div className="card">
        <div className="card-body">
          <h1 className="card-title text-center">Create a Player</h1>
          <form onSubmit={handleSubmit}>
            <div className="mb-3">
              <label htmlFor="playerName" className="form-label">
                Player Name
              </label>
              <input
                type="text"
                className="form-control"
                id="playerName"
                value={playerName}
                onChange={(e) => setPlayerName(e.target.value)}
                required
              />
            </div>
            <div className="mb-3">
              <label htmlFor="playerNationality" className="form-label">
                Player Nationality
              </label>
              <select
                className="form-control"
                id="playerNationality"
                value={playerNationality}
                onChange={(e) => setPlayerNationality(e.target.value)}
                required
              >
                <option value="">Select Nationality</option>
                {nationalities.map((nationality) => (
                  <option key={nationality} value={nationality}>
                    {nationality}
                  </option>
                ))}
              </select>
            </div>
            <div className="mb-3">
              <label htmlFor="playerPosition" className="form-label">
                Player Position
              </label>
              <select
                className="form-control"
                id="playerPosition"
                value={playerPosition}
                onChange={(e) => setPlayerPosition(e.target.value)}
                required
              >
                <option value="">Select Position</option>
                {positions.map((position) => (
                  <option key={position} value={position}>
                    {position}
                  </option>
                ))}
              </select>
            </div>
            <div className="text-center">
              <button type="submit" className="btn btn-primary">
                Create Player
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default PlayerCreator;