# fsharp-binder-template

Hit the ground running with a basic template for F# [Jupyter notebooks](https://jupyter.org/) on [binder](https://mybinder.org/).

Supports F# 8 💪

Try out this template on binder:

[![Binder](https://mybinder.org/badge_logo.svg)](https://mybinder.org/v2/gh/jovaneyck/fsharp-binder-template/HEAD)

## How to use

1. [fork](https://docs.github.com/en/get-started/quickstart/fork-a-repo) this repository
2. add your notebooks in the "notebooks" folder. There's some examples available
4. go to [mybinder.org](https://mybinder.org/), paste the url of your forked repository and hit "launch"
5. have fun with your online interactive Jupyter notebooks with F# support ♥

## Read more about it
On my blog [jovaneyck.be - F# notebooks](https://jvaneyck.wordpress.com/2021/07/16/fsharp-notebooks/)

## Running the docker image locally

```bash
docker build . -t binder
docker run -p8888:8888 binder

#browse to the 127.0.0.1 url provided in the logs, e.g. http://127.0.0.1:8888/lab?token=4fa98bd64e4c6b917d06700a375936fd7b86c87d1d123c8c :
#
#[C 2024-03-05 09:50:22.619 ServerApp]
#
#    To access the server, open this file in a browser:
#        file:///home/jovyan/.local/share/jupyter/runtime/jpserver-7-open.html
#    Or copy and paste one of these URLs:
#        http://2e1b702a0fff:8888/lab?token=4fa98bd64e4c6b917d06700a375936fd7b86c87d1d123c8c
#        http://127.0.0.1:8888/lab?token=4fa98bd64e4c6b917d06700a375936fd7b86c87d1d123c8c

```
