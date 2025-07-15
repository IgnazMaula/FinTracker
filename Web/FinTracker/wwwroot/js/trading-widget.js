// --------------------
// TRADING VIEW WIDGET
// --------------------

// Screener Stock
window.loadStockScreener = function (market) {
    const containerId = "tradingview-stock-widget-container";
    const container = document.getElementById(containerId);

    if (!container) return;

    container.innerHTML = ""; // Clear any existing content

    const widgetContainer = document.createElement("div");
    widgetContainer.className = "tradingview-widget-container";

    const widgetInner = document.createElement("div");
    widgetInner.className = "tradingview-widget-container__widget";

    const copyright = document.createElement("div");
    copyright.className = "tradingview-widget-copyright";
    copyright.innerHTML = `
        <a href="https://www.tradingview.com/" rel="noopener nofollow" target="_blank">
            <span class="blue-text">Track all markets on TradingView</span>
        </a>
    `;

    const marketMap = {
        "us": "america",
        "indonesia": "indonesia",
        "australia": "australia",
        "forex": "forex"
    };

    const mappedMarket = marketMap[market] || "america"; // fallback to US

    const script = document.createElement("script");
    script.type = "text/javascript";
    script.async = true;
    script.src = "https://s3.tradingview.com/external-embedding/embed-widget-screener.js";
    script.innerHTML = JSON.stringify({
        width: "100%",
        height: "100%",
        defaultColumn: "overview",
        defaultScreen: "most_capitalized",
        showToolbar: false,
        locale: "en",
        market: mappedMarket,
        colorTheme: "light"
    });

    widgetContainer.appendChild(widgetInner);
    widgetContainer.appendChild(copyright);
    widgetContainer.appendChild(script);
    container.appendChild(widgetContainer);
};

// Screener Cryptocurrency
window.loadCryptoScreener = function () {
    const containerId = "tradingview-crypto-widget-container";
    const container = document.getElementById(containerId);
    if (!container) return;

    container.innerHTML = "";

    const widgetContainer = document.createElement("div");
    widgetContainer.className = "tradingview-widget-container";

    const widgetInner = document.createElement("div");
    widgetInner.className = "tradingview-widget-container__widget";

    const copyright = document.createElement("div");
    copyright.className = "tradingview-widget-copyright";
    copyright.innerHTML = `
        <a href="https://www.tradingview.com/" rel="noopener nofollow" target="_blank">
            <span class="blue-text">Track all markets on TradingView</span>
        </a>
    `;

    const script = document.createElement("script");
    script.type = "text/javascript";
    script.async = true;
    script.src = "https://s3.tradingview.com/external-embedding/embed-widget-screener.js";
    script.innerHTML = JSON.stringify({
        defaultColumn: "overview",
        screener_type: "crypto_mkt",
        displayCurrency: "USD",
        colorTheme: "light",
        isTransparent: false,
        locale: "en",
        width: "100%",
        height: "100%",
    });

    widgetContainer.appendChild(widgetInner);
    widgetContainer.appendChild(copyright);
    widgetContainer.appendChild(script);
    container.appendChild(widgetContainer);
};

// Screener Forex
window.loadForexScreener = function () {
    const containerId = "tradingview-forex-widget-container";
    const container = document.getElementById(containerId);

    if (!container) return;

    container.innerHTML = ""; // Clear any existing content

    const widgetContainer = document.createElement("div");
    widgetContainer.className = "tradingview-widget-container";

    const widgetInner = document.createElement("div");
    widgetInner.className = "tradingview-widget-container__widget";

    const copyright = document.createElement("div");
    copyright.className = "tradingview-widget-copyright";
    copyright.innerHTML = `
        <a href="https://www.tradingview.com/" rel="noopener nofollow" target="_blank">
            <span class="blue-text">Track all markets on TradingView</span>
        </a>
    `;

    const script = document.createElement("script");
    script.type = "text/javascript";
    script.async = true;
    script.src = "https://s3.tradingview.com/external-embedding/embed-widget-screener.js";
    script.innerHTML = JSON.stringify({
        width: "100%",
        height: "100%",
        defaultColumn: "overview",
        defaultScreen: "top_gainers",
        showToolbar: false,
        locale: "en",
        market: "forex",
        colorTheme: "light"
    });

    widgetContainer.appendChild(widgetInner);
    widgetContainer.appendChild(copyright);
    widgetContainer.appendChild(script);
    container.appendChild(widgetContainer);
};


