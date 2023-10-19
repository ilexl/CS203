from flask import Flask, render_template
import os

app = Flask(__name__, static_folder="./", template_folder="./", static_url_path="")

@app.route('/', methods=["POST", "GET"]) 
def home():
  return render_template("index.html"), 200

if __name__ == "__main__":
  port = int(os.environ.get("PORT", 5000))
  app.run(debug=True, port=port)