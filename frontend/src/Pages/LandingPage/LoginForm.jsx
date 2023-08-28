import { useState } from "react"
import SignIn from "../../Components/SignIn/SignIn";
import { useNavigate } from "react-router-dom";
import { useCookies } from 'react-cookie';

const GetUserLevel = async () => {
    return await fetch(process.env.REACT_APP_API_URL + "/level")
        .then(res => res.text())
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
        const userLevel = await GetUserLevel();
        console.log(userLevel)
        if(auth.status === 401) {
            setShowMsg(false);
            console.error("Wrong login credentials!")
        }
        else if(auth.status === 500){
            console.error("Can't communicate with server!")
        }
        else if(auth.status === 200){
            setCookie("token", await auth.token)
            setCookie("username", username)
            setCookie("userlevel", userLevel)
            console.log("Login successful!")
            navigate("/home");
        }
    }
    
    return <SignIn onSubmit={onSubmit} showMsg={showMsg}></SignIn>
}

export default LoginForm