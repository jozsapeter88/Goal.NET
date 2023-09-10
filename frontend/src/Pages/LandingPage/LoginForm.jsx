import { useContext, useState } from "react"
import SignIn from "../../Components/SignIn/SignIn";
import { useNavigate } from "react-router-dom";
import { useCookies } from 'react-cookie';
import { UserContext } from "../../Contexts/UserContext";

const GetUserLevel = async (token) => {
    return await fetch(process.env.REACT_APP_API_URL + "/user/level", {
        headers: {
            'Authorization': "Bearer " + token
        }
    })
        .then(res => res.text())}

const GetUserLevels = async (token) => {
    const response = await fetch(process.env.REACT_APP_API_URL + `/user/levels`);
        if (response.ok) {
          const data = await response.json();
          return data;
        } else {
          console.error("Error fetching levels:", response.statusText);
        }
}

const Authorize = async (username, password) => {
    const loginObj = {"UserName": username, "Password": password}
    const response = await fetch(process.env.REACT_APP_API_URL + "/user/login", {
        method: "POST", 
        headers: {
            "Content-Type": "application/json"
        }, 
        credentials: "include",
        body: JSON.stringify(loginObj)
    })
     return response;
}

const LoginForm = () => {
    const [cookies, setCookie, removeCookie] = useCookies(["token", "username", "userlevel", "userlevels"]);
    const navigate = useNavigate();
    const [showMsg, setShowMsg] = useState(true);
    const { login } = useContext(UserContext);

    const onSubmit = async (e) => {
        setShowMsg(true);
        e.preventDefault();
        const username = e.target.formBasicUsername.value;
        const password = e.target.formBasicPassword.value;
        const auth = await Authorize(username, password);

        if (auth.status === 401) {
            setShowMsg(false);
            console.error("Wrong login credentials!");
        } else if (auth.status === 500) {
            console.error("Can't communicate with the server!");
        } else if (auth.status === 200) {
            const { token, user } = await auth.json();
            console.log("Login successful!");
            console.log(token);
            setCookie("token", token);
            setCookie("username", username);
            const userLevel = await GetUserLevel(token);
            const userLevels = await GetUserLevels(token);
            console.log(user)
            localStorage.setItem('user', JSON.stringify(user));
         
            login(user); // Assuming setUser is a function to set the user in your context
            setCookie("userlevel", userLevel);
            setCookie("userlevels", userLevels);
            console.log(userLevels);
            navigate("/home");
        }
    }

    return <SignIn onSubmit={onSubmit} showMsg={showMsg}></SignIn>;
}

export default LoginForm;






