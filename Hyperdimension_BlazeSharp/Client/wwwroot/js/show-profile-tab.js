export function showProfileTab(navTabId, tabPaneId) {

    const navTabs = document.querySelectorAll('#nav-tab > .nav-link');

    for (let i = 0; i < navTabs.length; i++) {
        navTabs[i].className = navTabs[i].className.replace(" active", "");
    }

    const tabPanes = document.getElementsByClassName("tab-pane");

    for (let i = 0; i < tabPanes.length; i++) {
        tabPanes[i].className = tabPanes[i].className.replace(" show active", "");
    }

    document.getElementById(navTabId).className += " active";
    document.getElementById(tabPaneId).className += " show active";
}
