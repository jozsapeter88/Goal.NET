import {useState, createContext} from 'react';

export const UserContext = createContext();

export const UserContextProvider = (props) => {
    const [user, setUser] = useState({
        id: null,
        username: null,
        points: null,
        teams: [],
    });
    const login = (userData) => {
       
        setUser(userData);
      };
    
      const logout = () => {
       
        setUser(null);
      };
    
    return (
        <UserContext.Provider value={{user,login, logout, setUser}}>
        {props.children}
        </UserContext.Provider>
    );
    }