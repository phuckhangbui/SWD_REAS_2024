// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAuth, GoogleAuthProvider } from "firebase/auth"
import { getFirestore } from "firebase/firestore";

// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
const firebaseConfig = {
  apiKey: "AIzaSyDKbHnbJZiHru7lftkNJq75bxyLvy2Cj6I",
  authDomain: "expense-tracker-e85dd.firebaseapp.com",
  projectId: "expense-tracker-e85dd",
  storageBucket: "expense-tracker-e85dd.appspot.com",
  messagingSenderId: "875637994577",
  appId: "1:875637994577:web:2a1263f634c0727b197470"
};
//sau này có dùng thì sửa lại các attribute trên

// Initialize Firebase
const app = initializeApp(firebaseConfig);
export const auth = getAuth(app);
export const provider = new GoogleAuthProvider();
export const db = getFirestore(app)