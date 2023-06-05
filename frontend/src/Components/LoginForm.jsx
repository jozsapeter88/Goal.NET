import { useState } from "react"
import SignIn from "../Pages/SignIn/SignIn"
import { useNavigate } from "react-router-dom";
import Cookies from "universal-cookie";

const Authorize = async (username, password) => {
    let status;
    let token;
    const loginObj = {"UserName": username, "Password": password}
    await fetch("/api/user/login", {
        method: "POST", 
        headers: {
            "Content-Type": "application/json",
            // 'Accept': ''
        }, 
        body: JSON.stringify(loginObj)
    }).then(res => {
        status = res.status;
        token = res.body;
    })
    return {"status": status, "token": token};
}

const LoginForm = () => {
    const navigate = useNavigate();
    const [showMsg, setShowMsg] = useState(true)

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
            Cookies.set("token", auth.token)
            console.log("Login successful!")
            navigate("/home");
        }
    }
    
    return <SignIn onSubmit={onSubmit} showMsg={showMsg}></SignIn>
}

export default LoginForm