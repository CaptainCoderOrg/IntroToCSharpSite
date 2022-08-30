/**
 * Finds the Top Pane and scrolls to the top.
 */
function ScrollToTop() {
    const topPanes = document.getElementsByClassName("split-pane__top");
    for (let p of topPanes) p.scrollTop = 0;
}

/**
 * Finds the specified element and scrolls it into view.
 */
 function ScrollIntoView(id) {
    const element = document.getElementById(id);
    if (!element) throw `Unable to scroll to element ${id} because it could not be found.`;
    const yOffset = -10;
    const pane = document.getElementById("VerticalSplitPane-Top-0");
    const y = element.getBoundingClientRect().top + window.pageYOffset + yOffset - pane.offsetTop;
    pane.scrollTo({top: y, behavior: 'smooth'});
}

/**
 *
 * @returns true if the client is mobile and false otherwise
 */
function IsMobileClient() {
    const agent = navigator.userAgent;
    if( /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(agent) ) {
        return true;
   }
   return false;
}

/**
 *
 * @returns The client information
 */
function GetClient() {
    return navigator.userAgent;
}