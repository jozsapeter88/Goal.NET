import { useContext, useEffect } from "react";
import { UserContext } from "./Contexts/UserContext";
import { fetchCurrentUser } from "./Pages/TeamManager/TeamManager";
import { UserContextProvider } from "./Contexts/UserContext";
import { useCookies } from 'react-cookie';
import { Routes, Route } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import UserEditor from './Components/AdminTools/UserEditor';
import HomePage from './Pages/Home/HomePage';
import LoginForm from './Pages/LandingPage/LoginForm';
import RegisterForm from './Components/RegisterForm';
import PlayerCreator from './Components/PlayerCreator/PlayerCreator';
import PlayerList from './Components/PlayerList/PlayerList';
import TeamManager from './Pages/TeamManager/TeamManager';
import AddPlayerList from "./Components/AddPlayerList/AddPlayerList";
const App = () => {

const navigate = useNavigate();

const { user, login } = useContext(UserContext);
console.log(user)
useEffect(() => {
  const userJSON = localStorage.getItem('user');
  console.log(userJSON)
  if (userJSON) {
    const userData = JSON.parse(userJSON);
    login(userData); // Log the user in using the data from localStorage
  }
}, []);

    return (
        <div className="App">
        <Routes>
        <Route path="/" element= {<LoginForm />} />
        <Route path="register" element={<RegisterForm/>} />
        <Route path="home" element={<HomePage />} />
        <Route path="createPlayer" element={<PlayerCreator/>} />
        <Route path="playerList" element={<PlayerList/>} />
        <Route path="userEditor" element={<UserEditor/>} />
        <Route path="teamManager" element={ <TeamManager/>} />
        <Route path="teamList" element={<AddPlayerList/>} />
        <Route path="admintool" element={<UserEditor/>} />
      </Routes>
        </div>
    )
}
const AppWithUserContext = () => (
    <UserContextProvider>
      <App />
    </UserContextProvider>
  );
  
  export default AppWithUserContext;
  


