import "./App.css";
import {
  BrowserRouter as Router,
  Route,
  Routes,
  Navigate,
  redirect,
} from "react-router-dom";
import PageNotFound from "./Pages/PageNotFound";
import { AdminLayout } from "./Pages/Admin/AdminLayout";
import AdminDashboard from "./Pages/Admin/AdminDashboard";
import AuctionOngoing from "./Pages/Admin/AdminAuctionOngoing";
import AdminStaffList from "../src/Pages/Admin/StaffList/StaffList/index";
import AdminMemberList from "../src/Pages/Admin/MemberList/MemberList/index";
import AdminAddStaff from "../src/Pages/Admin/AdminCreateStaff/AdminCreateStaff";
import PendingList from "../src/Pages/Admin/AdminRealEstatePending";
import AllList from "../src/Pages/Admin/AdminRealEstateAll"
import NewsList from "../src/Pages/Admin/AdminNews"
import AdminRule from "../src/Pages/Admin/AdminRule"
import AddRule from "../src/Pages/Admin/AdminAddRule"
import AdminCreateNews from "../src/Pages/Admin/AdminCreateNews"
import AuctionComplete from "../src/Pages/Admin/AdminAuctionComplete";
import AuctionDetail from "./Pages/Admin/AdminAuctionDetail";
import HomePage from "./Pages/Member/HomePage/homePage";
import RealEstatePage from "./Pages/Member/RealEstatePage/realEstatePage";
import HelpPage from "./Pages/Member/HelpPage/helpPage";
import MemberLayout from "./Pages/Member/memberLayout";
import AuctionPage from "./Pages/Member/AuctionPage/auctionPage";
import NewsPage from "./Pages/Member/NewsPage/newsPage";
import SellPage from "./Pages/Member/SellPage/sellPage";
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
              <Route path="user/staff" element={<AdminStaffList />} />
              <Route path="user/member" element={<AdminMemberList />} />
              <Route path="user/create" element={<AdminAddStaff/>}/>
              <Route path="real-estate/pending" element={<PendingList/>}/>
              <Route path="real-estate/all" element={<AllList/>}/>
              <Route path="news" element={<NewsList/>}/>
              <Route path="news/create" element={<AdminCreateNews/>}/>
              <Route path="term" element={<AdminRule/>}/>
              <Route path="term/create" element={<AddRule/>}/>
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
