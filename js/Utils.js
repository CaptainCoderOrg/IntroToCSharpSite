/**
 * Finds the Top Pane and scrolls to the top.
 */
function ScrollToTop() {
    const topPanes = document.getElementsByClassName("split-pane__top");
    for (let p of topPanes) p.scrollTop = 0;
}