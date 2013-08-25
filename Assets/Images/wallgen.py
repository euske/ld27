#!/usr/bin/env python
import sys
import pygame

pygame.font.init()
img = pygame.Surface((100, 1000), pygame.SRCALPHA)
font = pygame.font.Font('/usr/share/fonts/TTF/Vera.ttf', 24)
img.fill((0,0,0,0))
img.fill((255,0,0,255), (0, 900, 100, 100))
for i in xrange(10):
    if i == 0:
        s = 'GOAL'
        color = (255,255,0,255)
    else:
        s = '%3d' % (100-i*10)
        color = (255,255,255,255)
    t = font.render(s, 1, color)
    img.blit(t, (0,i*96))
    img.fill(color, (60, i*96+6, 40, 1))
pygame.image.save(img, 'wall.png')
