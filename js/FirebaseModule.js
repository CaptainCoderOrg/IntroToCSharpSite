import * as firebase from 'https://www.gstatic.com/firebasejs/9.6.6/firebase-app.js';
import * as firebase_auth from 'https://www.gstatic.com/firebasejs/9.6.6/firebase-auth.js';

const firebaseConfig = {
    apiKey: "AIzaSyAXkmsVawWcw_tRO5IGs1L-bMiTzEcaAS4",
    authDomain: "introtocsharp-a5eeb.firebaseapp.com",
    projectId: "introtocsharp-a5eeb",
    storageBucket: "introtocsharp-a5eeb.appspot.com",
    messagingSenderId: "842722285092",
    appId: "1:842722285092:web:1a8d4ed685199a89e7aab1"
};

// Initialize Firebase
const app = firebase.initializeApp(firebaseConfig);
// const auth = firebase_auth.initializeAuth(app);
const auth = firebase_auth.getAuth();

/**
 * Register a Blazor object to be notified when the auth state
 * changes.
 * @param {*} obj - Blazor object to notify 
 */
export function onAuthStateChanged(obj) {
    firebase_auth.onAuthStateChanged(auth, (user) => {
        obj.invokeMethodAsync("UpdateUser", JSON.stringify(user));
    });
}

/**
 * Invoke a Google Authentication popup
 */
export function firebaseLogin() {
    const provider = new firebase_auth.GoogleAuthProvider();
    const auth = firebase_auth.getAuth();
    firebase_auth.signInWithPopup(auth, provider)
      .then((result) => {
        // TODO: Redirect?
      }).catch((error) => {
        // TODO: Redirect?
        console.error("Error!");
        console.error(error);
      });
}

/**
 * De-authenticate user
 */
export function firebaseLogout() {
    const auth = firebase_auth.getAuth();
    firebase_auth.signOut(auth).then(() => {
      // Sign-out successful.
    }).catch((error) => {
      // An error happened.
    });
}

// Expose functions for calling in Blazor App
window.firebaseLogin = firebaseLogin;
window.firebaseLogout = firebaseLogout;
window.onAuthStateChanged = onAuthStateChanged;