import { useState } from "react";
import SignUp from "./SignUp/SignUp";
import { useNavigate } from "react-router-dom";

const Authorize = async (username, password) => {
  const loginObj = { UserName: username, Password: password };
  return await fetch(process.env.REACT_APP_API_URL + "/user/register", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(loginObj),
  }).then((res) => {
    return { status: res.status, token: res.text() };
  });
};

function timeout(delay) {
  return new Promise((res) => setTimeout(res, delay));
}

const RegisterForm = () => {
  const navigate = useNavigate();
  const [showMsg, setShowMsg] = useState(true);
  const [successfulReg, setReg] = useState(false);

  const onSubmit = async (e) => {
    e.preventDefault();
    const username = e.target.formBasicUsername.value;
    const password = e.target.formBasicPassword.value;
    const auth = await Authorize(username, password);
    if (auth.status === 401) {
      setShowMsg(false);
      console.error("Username is already taken!");
    } else if (auth.status === 500) {
      console.error("Can't communicate with server!");
    } else if (auth.status === 200) {
      setShowMsg(true);
      setReg(true);
      console.log("Registration successful!");
      await timeout(1000);
      navigate("/");
    }
  };

  return (
    <SignUp
      onSubmit={onSubmit}
      showMsg={showMsg}
      successfulReg={successfulReg}
    ></SignUp>
  );
};

export default RegisterForm;
