// Sidebar collapse toggle & mobile menu toggles — plain JS on purpose, same reasoning as theme.js:
// pure client-side UI state, persisted to localStorage where applicable and stamped as a class on <html>.
(function () {
    var STORAGE_KEY = 'fa-sidebar-collapsed';
    var CLASS_NAME = 'fa-sidebar-collapsed';
    var SETTLED_CLASS_NAME = 'fa-sidebar-collapsed-settled';
    // Matches .fa-sidebar-link-text's max-width transition duration in fa-styles.css —
    // justify-content isn't an animatable CSS property, so centering the icon has to
    // be a discrete class added once the label has actually finished collapsing,
    // not the instant collapse starts (that read as the icon jumping to center
    // before the text had visually finished closing over).
    var SETTLE_DELAY_MS = 200;
    var settleTimer = null;

    function apply(collapsed, immediate) {
        document.documentElement.classList.toggle(CLASS_NAME, collapsed);
        var buttons = document.querySelectorAll('[data-sidebar-toggle]');
        for (var i = 0; i < buttons.length; i++) {
            buttons[i].setAttribute('aria-expanded', collapsed ? 'false' : 'true');
        }

        if (settleTimer) {
            clearTimeout(settleTimer);
            settleTimer = null;
        }

        if (!collapsed) {
            // Expanding: un-center immediately so the icon moves back to the left as
            // the label grows back in, instead of staying centered mid-animation.
            document.documentElement.classList.remove(SETTLED_CLASS_NAME);
            return;
        }

        if (immediate) {
            document.documentElement.classList.add(SETTLED_CLASS_NAME);
        } else {
            settleTimer = setTimeout(function () {
                document.documentElement.classList.add(SETTLED_CLASS_NAME);
                settleTimer = null;
            }, SETTLE_DELAY_MS);
        }
    }

    window.faToggleSidebar = function () {
        var collapsed = document.documentElement.classList.contains(CLASS_NAME);
        var next = !collapsed;
        localStorage.setItem(STORAGE_KEY, next ? '1' : '0');
        apply(next, false);
    };

    // Mobile off-canvas sidebar open/closed — a separate axis from the desktop icon-rail
    // collapse above (CLASS_NAME/apply()). Not persisted to localStorage: unlike
    // the desktop rail, staying open across page loads on a small screen would
    // just mean every new page starts covered by the menu.
    var MOBILE_CLASS_NAME = 'fa-sidebar-mobile-open';

    window.faToggleSidebarMobile = function () {
        var open = document.documentElement.classList.toggle(MOBILE_CLASS_NAME);
        var buttons = document.querySelectorAll('[data-sidebar-mobile-toggle]');
        for (var i = 0; i < buttons.length; i++) {
            buttons[i].setAttribute('aria-expanded', open ? 'true' : 'false');
        }
    };

    // Mobile topbar nav menu open/closed
    var NAV_MOBILE_CLASS_NAME = 'fa-header-nav-mobile-open';

    window.faToggleNavMobile = function () {
        var open = document.documentElement.classList.toggle(NAV_MOBILE_CLASS_NAME);
        var buttons = document.querySelectorAll('[data-nav-mobile-toggle]');
        for (var i = 0; i < buttons.length; i++) {
            buttons[i].setAttribute('aria-expanded', open ? 'true' : 'false');
        }
    };

    // Mobile subnav menu open/closed
    var SUBNAV_MOBILE_CLASS_NAME = 'fa-subnav-mobile-open';

    window.faToggleSubNavMobile = function (btn) {
        var subnav = btn ? btn.closest('.fa-subnav') : document.querySelector('.fa-subnav');
        if (subnav) {
            var open = subnav.classList.toggle(SUBNAV_MOBILE_CLASS_NAME);
            var toggleBtn = subnav.querySelector('[data-subnav-mobile-toggle]');
            if (toggleBtn) {
                toggleBtn.setAttribute('aria-expanded', open ? 'true' : 'false');
            }
        }
    };

    // Auto-close mobile menus on link click or outside click
    document.addEventListener('click', function (e) {
        // Nav mobile menu closing
        if (document.documentElement.classList.contains(NAV_MOBILE_CLASS_NAME)) {
            if (!e.target.closest('[data-nav-mobile-toggle]')) {
                if (e.target.closest('.fa-header-nav a') || e.target.closest('.fa-header-nav button') || e.target.closest('.fa-header-subnav-row a') || e.target.closest('.fa-header-subnav-row button') || e.target.closest('.fa-subnav-link') || e.target.closest('.fa-subnav-button') || !e.target.closest('.fa-header-collapse')) {
                    document.documentElement.classList.remove(NAV_MOBILE_CLASS_NAME);
                    var navButtons = document.querySelectorAll('[data-nav-mobile-toggle]');
                    for (var n = 0; n < navButtons.length; n++) {
                        navButtons[n].setAttribute('aria-expanded', 'false');
                    }
                }
            }
        }

        // Subnav mobile menu closing
        var openSubnavs = document.querySelectorAll('.fa-subnav.' + SUBNAV_MOBILE_CLASS_NAME);
        for (var i = 0; i < openSubnavs.length; i++) {
            var subnav = openSubnavs[i];
            if (!e.target.closest('[data-subnav-mobile-toggle]')) {
                if (e.target.closest('.fa-subnav-link') || e.target.closest('.fa-subnav-button') || !e.target.closest('.fa-subnav')) {
                    subnav.classList.remove(SUBNAV_MOBILE_CLASS_NAME);
                    var subnavBtn = subnav.querySelector('[data-subnav-mobile-toggle]');
                    if (subnavBtn) {
                        subnavBtn.setAttribute('aria-expanded', 'false');
                    }
                }
            }
        }

        // Sidebar mobile menu closing on navigation click
        if (document.documentElement.classList.contains(MOBILE_CLASS_NAME)) {
            if (!e.target.closest('[data-sidebar-mobile-toggle]') && e.target.closest('.fa-sidebar a')) {
                document.documentElement.classList.remove(MOBILE_CLASS_NAME);
                var sideButtons = document.querySelectorAll('[data-sidebar-mobile-toggle]');
                for (var s = 0; s < sideButtons.length; s++) {
                    sideButtons[s].setAttribute('aria-expanded', 'false');
                }
            }
        }
    });

    document.addEventListener('DOMContentLoaded', function () {
        // Applying the saved state on page load should never animate — only the
        // interactive toggle click gets the delayed settle.
        apply(localStorage.getItem(STORAGE_KEY) === '1', true);
    });
})();
