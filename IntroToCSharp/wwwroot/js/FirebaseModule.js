import * as firebase from 'https://www.gstatic.com/firebasejs/9.6.6/firebase-app.js';
import * as firebase_auth from 'https://www.gstatic.com/firebasejs/9.6.6/firebase-auth.js';
import * as firebase_database from "https://www.gstatic.com/firebasejs/9.6.6/firebase-database.js"

const firebaseConfig = {
    apiKey: "AIzaSyAXkmsVawWcw_tRO5IGs1L-bMiTzEcaAS4",
    authDomain: "introtocsharp-a5eeb.firebaseapp.com",
    projectId: "introtocsharp-a5eeb",
    storageBucket: "introtocsharp-a5eeb.appspot.com",
    messagingSenderId: "842722285092",
    appId: "1:842722285092:web:1a8d4ed685199a89e7aab1",
    databaseURL: "https://introtocsharp-a5eeb-default-rtdb.firebaseio.com/",
};

// Initialize Firebase
const app = firebase.initializeApp(firebaseConfig);
// const auth = firebase_auth.initializeAuth(app);
const auth = firebase_auth.getAuth();
const database = firebase_database.getDatabase(app);

let _user = undefined;

/**
 * Register a Blazor object to be notified when the auth state
 * changes.
 * @param {*} obj - Blazor object to notify
 */
export function onAuthStateChanged(obj) {
    firebase_auth.onAuthStateChanged(auth, (user) => {
        _user = user;
        obj.invokeMethodAsync("UpdateUser", JSON.stringify(user));
    });
}


function firebaseLogin(provider) {
    if (_user) return; // If we're already logged in, noop
    // const provider = providernew firebase_auth.GoogleAuthProvider();
    const auth = firebase_auth.getAuth();
    firebase_auth.signInWithPopup(auth, provider)
        .then((result) => {}).catch((error) => {
            console.error("Error!");
            console.error(error);
        });
}

/**
 * Invoke a Login with Email and Password
 */
export function firebaseEmailLogin(email, password) {
    console.log(`Authenticating with email: ${email} ${password}`)
    const auth = firebase_auth.getAuth();
    firebase_auth.signInWithEmailAndPassword(auth, email, password)
        .then((result) => { console.log("Authentication Successful."); }).catch((error) => {
            console.error("Error!");
            console.error(error);
        });
}

/**
 * Invoke a Google Authentication popup
 */
export function firebaseGoogleLogin() {
    firebaseLogin(new firebase_auth.GoogleAuthProvider());
}

/**
 * Invoke a GitHub Authentication popup
 */
export function firebaseGitHubLogin() {
    firebaseLogin(new firebase_auth.GithubAuthProvider());
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

/**
 * Given a data path queries the database. Each time the data is updated,
 * the provided callback receives a DataSnapshot.
 * @param {string} path
 * @param {function} callback
 */
export function refDatabase(path, handler) {
    const ref = firebase_database.ref;
    const onValue = firebase_database.onValue;
    const observer = ref(database, path)
    onValue(observer, (snapshot) => {
        let val = snapshot.val();
        val = val == null ? null : JSON.stringify(val);
        handler.invokeMethodAsync("OnChange", val);
    });
}

/**
 * Given a path and data to write, attempts to write the data at the
 * specified path.
 * @param {string} path
 * @param {JSON} data
 * @returns A Promise containing the result of this operation
 */
export function setDatabase(path, data, handler) {
    const set = firebase_database.set;
    const ref = firebase_database.ref;
    set(ref(database, path), JSON.parse(data))
        .then(() => handler.invokeMethodAsync("OnSuccess"))
        .catch((exception) => handler.invokeMethodAsync("OnException", JSON.stringify(exception)));
}

/**
 * Given a path, attempts to remove it from the database.
 * @param {string} path
 * @returns A Promise containing the result of this operation
 */
export function removeDatabase(path, handler) {
    const remove = firebase_database.remove;
    const ref = firebase_database.ref;
    const result = remove(ref(database, path));
    result.then(() => handler.invokeMethodAsync("OnSuccess"))
        .catch((exception) => handler.invokeMethodAsync("OnException", JSON.stringify(exception)));
}

// Expose functions for calling in Blazor App
window.firebaseGoogleLogin = firebaseGoogleLogin;
window.firebaseGitHubLogin = firebaseGitHubLogin;
window.firebaseEmailLogin = firebaseEmailLogin;
window.firebaseLogout = firebaseLogout;
window.onAuthStateChanged = onAuthStateChanged;
window.refDatabase = refDatabase;
window.setDatabase = setDatabase;
window.removeDatabase = removeDatabase;