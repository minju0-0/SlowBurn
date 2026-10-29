// Small progressive-enhancement script: fade sections in on scroll.
// Add class="reveal" to any element. If JS/IntersectionObserver is unavailable, it stays visible.
(function () {
    "use strict";
    function initReveal() {
        var items = document.querySelectorAll(".reveal");
        if (!items.length || !("IntersectionObserver" in window)) return;
        var observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (e) {
                if (e.isIntersecting) {
                    e.target.classList.remove("reveal-init");
                    e.target.classList.add("in-view");
                    observer.unobserve(e.target);
                }
            });
        }, { threshold: 0.12 });
        items.forEach(function (el) { el.classList.add("reveal-init"); observer.observe(el); });
    }
    document.addEventListener("DOMContentLoaded", initReveal);
    // Re-run after Blazor enhanced navigation
    if (window.Blazor) { window.Blazor.addEventListener("enhancedload", initReveal); }

    // Synthesized gentle chime for the brew timer using Web Audio API
    window.playTimerChime = function () {
        try {
            var AudioCtx = window.AudioContext || window.webkitAudioContext;
            if (!AudioCtx) return;
            var ctx = new AudioCtx();
            var osc = ctx.createOscillator();
            var gain = ctx.createGain();
            osc.type = "sine";
            osc.frequency.setValueAtTime(587.33, ctx.currentTime); // D5
            osc.frequency.setValueAtTime(880, ctx.currentTime + 0.15); // A5
            gain.gain.setValueAtTime(0.2, ctx.currentTime);
            gain.gain.exponentialRampToValueAtTime(0.001, ctx.currentTime + 0.8);
            osc.connect(gain);
            gain.connect(ctx.destination);
            osc.start();
            osc.stop(ctx.currentTime + 0.85);
        } catch (e) {
            // Audio context not allowed without interaction or unsupported
        }
    };
})();

