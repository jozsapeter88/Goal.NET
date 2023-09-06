import { useState } from "react"
import SignIn from "../../Components/SignIn/SignIn";
import { useNavigate } from "react-router-dom";
import { useCookies } from 'react-cookie';

const GetUserLevel = async (token) => {
    return await fetch(process.env.REACT_APP_API_URL + "/user/level", {
        headers: {
            'Authorization': "Bearer " + token
        }
    })
        .then(res => res.text())
}

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
    return await fetch(process.env.REACT_APP_API_URL + "/user/login", {
        method: "POST", 
        headers: {
            "Content-Type": "application/json"
        }, 
        credentials: "include",
        body: JSON.stringify(loginObj)
    }).then(res => {
       return  {"status": res.status, "token": res.text()}; 
    })
     
}

const LoginForm = () => {
    const [cookies, setCookie, removeCookie] = useCookies(true);
    const navigate = useNavigate();
    const [showMsg, setShowMsg] = useState(true)
console.log(process.env.REACT_APP_API_URL)
    const onSubmit = async (e) => {
        setShowMsg(true);
        e.preventDefault();
        const username = e.target.formBasicUsername.value;
        const password = e.target.formBasicPassword.value;
        const auth = await Authorize(username, password);
        if(auth.status === 401) {
            setShowMsg(false);
            console.error("Wrong login credentials!")
        }
        else if(auth.status === 500){
            console.error("Can't communicate with server!")
        }
        else if(auth.status === 200){
            console.log("Login successful!");
            setCookie("token", await auth.token);
            setCookie("username", username);
            const userLevel = await GetUserLevel(await auth.token);
            const userLevels = await GetUserLevels(await auth.token);
            setCookie("userlevel", userLevel);
            setCookie("userlevels", userLevels);
            navigate("/home");
        }
    }
    
    return <SignIn onSubmit={onSubmit} showMsg={showMsg}></SignIn>
}

export default LoginForm