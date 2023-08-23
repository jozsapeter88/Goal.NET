import "./HomePage.css";
import Menu from "../../Components/Menu/Menu";
import Dashboard from "../../Pages/Dashboard/Dashboard";
import Footer from "../../Components/Footer/Footer";

const HomePage = () => {
  return (
    <div>
      <Menu />
      <Dashboard />
      <Footer />
    </div>
  );
};

export default HomePage;
