import "./App.css";
import {
  BrowserRouter as Router,
  Route,
  Routes,
  Navigate,
  redirect,
} from "react-router-dom";
import PageNotFound from "./pages/PageNotFound";
import { AdminLayout } from "./pages/Admin/AdminLayout";
import AdminDashboard from "./pages/Admin/AdminDashboard";
import AuctionOngoing from "./pages/Admin/AdminAuctionOngoing";
import StaffList from "./pages/Admin/StaffList";
import MemberList from "./pages/Admin/MemberList";
import AuctionComplete from "./pages/Admin/AdminAuctionComplete";
import AuctionDetail from "./pages/Admin/AdminAuctionDetail";
import HomePage from "./pages/Member/HomePage/homePage";
import RealEstatePage from "./pages/Member/RealEstatePage/realEstatePage";
import HelpPage from "./pages/Member/HelpPage/helpPage";
import MemberLayout from "./pages/Member/memberLayout";
import AuctionPage from "./pages/Member/AuctionPage/auctionPage";
import NewsPage from "./pages/Member/NewsPage/newsPage";
import SellPage from "./pages/Member/SellPage/sellPage";
import { useContext } from "react";
import { UserContext } from "./context/userContext";
// import { Redirect } from "react-router-dom";

// function PrivateRoute({ element: Component, ...rest }) {
//   const { user } = useContext(UserContext);

//   // Check if user is authenticated and their role
//   if (!user) {
//     // If user is not logged in, redirect to login page
//     return redirect("/");
//   } else if (rest.role && user.roleId !== rest.role) {
//     // If user's role does not match the required role, redirect to homepage
//     return redirect("/");
//   } else {
//     // If user is authenticated and has correct role, render the component
//     return <Route {...rest} element={<Component />} />;
//   }
// }

function App() {
  const { user } = useContext(UserContext);

  return (
    <div className="App">
      <Router>
        <Routes>
          <Route path="/" element={<MemberLayout />}>
            <Route index element={<HomePage />} />
            <Route path="/realEstate" element={<RealEstatePage />} />
            <Route path="/auction" element={<AuctionPage />} />
            <Route path="/help" element={<HelpPage />} />
            <Route path="/news" element={<NewsPage />} />
            {/* <PrivateRoute path="/sell" element={<SellPage />} role={3} /> */}
            {/* {user && user.roleId === 3 && (
              <Route path="/sell" element={<SellPage />} />
            )} */}
            {/* {if(user && user.roleId === 3) {
              return <Route path="/sell" element={<SellPage />} />
            } else {
              return (
                <Redirect
                   to={{
                    path: "/",
                      state: {
                         from: props.location,
                      },
                   }}
                />
            }} */}
            <Route path="*" element={<PageNotFound />} />
          </Route>

          {/* {user && (user.roleId === 1 || user.roleId === 2) && ( */}
            <Route path="/admin" element={<AdminLayout />}>
              <Route index element={<AdminDashboard />} />
              <Route path="auction/ongoing" element={<AuctionOngoing />} />
              <Route path="auction/complete" element={<AuctionComplete />} />
              <Route path="auction/detail/:key" element={<AuctionDetail />} />
              <Route path="user/staff" element={<StaffList />} />
              <Route path="user/member" element={<MemberList />} />
              <Route path="*" element={<PageNotFound />} />
            </Route>
          {/* )} */}
          <Route path="*" element={<PageNotFound />} />
        </Routes>
      </Router>
    </div>
  );
}

export default App;
