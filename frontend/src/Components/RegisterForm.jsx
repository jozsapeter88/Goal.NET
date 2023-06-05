import { useState } from "react"
import SignUp from "../Pages/SignUp/SignUp"
import { useNavigate } from "react-router-dom";
import Cookies from "universal-cookie";

const Authorize = async (username, password) => {
    let status;
    let token;
    const loginObj = {"UserName": username, "Password": password}
    await fetch("/api/user/register", {
        method: "POST", 
        headers: {
            "Content-Type": "application/json",
            // 'Accept': ''
        }, 
        body: JSON.stringify(loginObj)
    }).then(res =>{
        status = res.status;
        token = res.body;
    })
    return {"status": status, "token": token};
}

const RegisterForm = () => {
    const navigate = useNavigate();
    const [showMsg, setShowMsg] = useState(true)

    const onSubmit = async (e) => {
        e.preventDefault();
        const username = e.target.formBasicUsername.value;
        const password = e.target.formBasicPassword.value;
        const auth = await Authorize(username, password);
        if(auth.status === 401) {
            setShowMsg(false);
            console.error("Username is already taken!")
        }
        else if(auth.status === 500){
            console.error("Can't communicate with server!")
        }
        else if(auth.status === 200){
            Cookies.set("token", auth.token)
            setShowMsg(true);
            console.log("Registration successful!")
            navigate("/");
        }
    }
    
    return <SignUp onSubmit={onSubmit} showMsg={showMsg}></SignUp>
}

export default RegisterForm