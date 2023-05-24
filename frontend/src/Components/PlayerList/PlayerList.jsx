import { useEffect, useState } from "react";
import Loading from '../Loading';


const fetchPlayers = (signal) => {
    return fetch('http://localhost:3000/api/players/getAllPlayers',  { signal }).then((res) => res.json());
}

const PlayerList = () => {
    const [loading, setLoading] = useState(true);
    const [players, setPlayer] = useState([]);

    useEffect(() => {
        const controller = new AbortController();
        fetchPlayers(controller.signal)
          .then((players) => {
            setLoading(false);
            setPlayer(players);
          })
          .catch((error) => {
            if (error.name !== "AbortError") {
              setPlayer([]);
              throw error;
            }
          });
        return () => controller.abort();
      }, []);
    
      if (loading) {
        return <Loading />;
      }

      return(
        <div>
            <h1>Players</h1>    
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
                    {players.map((player) => (
                        <tr key={player.id}>
                            <td>{player.name}</td>
                            <td>{player.position}</td>
                            <td>{player.nationality}</td>
                            <td>{player.score}</td>
                        </tr>))}
                </tbody>
            </table>
            </div>
      )
}

export default PlayerList;
